using UnityEngine;
using System.Collections.Generic;
using Assets.CitadelBuilder;
using System.IO;
using System;
using Assets.Elements;
using System.Linq;
using Assets.Buffs;

public class CitadelDamage
{
    public int Damage { get; set; }
    public IElement Element { get; set; }
    public string AttackingPlayer { get; set; }
    public CitadelDamage(int damage, IElement element, string attackingPlayer)
    {
        Damage = damage;
        Element = element;
        AttackingPlayer = attackingPlayer;
    }
}

public class PersistentData : Photon.MonoBehaviour, INetworkController
{
    #region Class Member Variables

    public bool DEBUG = true;

    public ScreenManager screenManager;
    public Settings settings;

    public HexNetwork hNetwork;
    public int arenaPosition { get; private set; }
    public bool setPositions = false;
    public GUISkin hexSkin;

    public Player CurrentPlayer { get; private set; }
    public List<Player> players;
    public int PlayersEliminated { get; private set;}

    public Citadel[] Citadels;
    public int CurrentCitadel { get; set; }

    public string DefaultCharacter { get; set; }
    public string SettingsFilePath
    {
        get
        {
            return Player.HexFolderPath + "\\" + "hex.settings";
        }
    }

    //dictionary for network damage to blocks, keys are viewIDs, values are damage to take
    public Dictionary<int, CitadelDamage> Blocks;
    public int AltarHealth { get; set; }


    public bool LastLife { get; set; }

    #endregion

    public class OpponentData
    {
        public int MaxHP { get; private set; }
        public int CurHP { get; set; }
        public int MaxAltarHP { get; private set; }
        public int CurAltarHP { get; set; }
        public int Level { get; private set; }
        public string Class { get; private set; }
        public bool isEliminated { get; set; }
        public Animation CurrentAnimation { get; set; }
        public Vector3 Position { get; set; }

        public Dictionary<Type, PEffect> Buffs { get; private set; }
        public Vector3 IceBeamInfo { get; set; }

        public OpponentData(string cls, int maxHP, int altMHP, int level)
        {
            Class = cls;
            MaxHP = maxHP;
            CurHP = maxHP;
            MaxAltarHP = altMHP;
            CurAltarHP = altMHP;
            Level = level;
            isEliminated = false;
            Buffs = new Dictionary<Type, PEffect>();
        }
    }

    //maps alias to opponentdata
    public Dictionary<String, OpponentData> Opponents;

    public static PersistentData Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("PersistentData is a singleton, but multiple instances exist.");
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
       
        AltarHealth = Altar.AltarHealth;
        Initialize();
        players = LoadPlayers();
        PlayersEliminated = 0;
        Blocks = new Dictionary<int, CitadelDamage>();
        Opponents = new Dictionary<String, OpponentData>();
        hNetwork = new HexNetwork();
        Citadels = new Citadel[3]; //This is the number of Citadels. No exceptions.
        hexSkin = Resources.Load("skins/MainSkin") as GUISkin;
        CurrentCitadel = -1;
        ReadSettings();
        screenManager = new ScreenManager();
    }

    public void WriteSettings()
    {
        File.WriteAllText(SettingsFilePath, DefaultCharacter);
    }
    private void ReadSettings()
    {
        try
        {
            DefaultCharacter = File.ReadAllText(SettingsFilePath);
        }
        catch (Exception)
        {
            //it's okay. Probably the first time loading.
        }
    }

    private void Update()
    {
        screenManager.Update();
        if (DEBUG && PhotonNetwork.connected && PhotonNetwork.room != null && !setPositions)
        {
            System.Random rnd = new System.Random();
            int map = rnd.Next(0, 2);
            //int map = 0;
            switch (map)
            {
                case 0:
                    Application.LoadLevel("ArenaBadlands");
                    break;
                case 1:
                    Application.LoadLevel("ArenaSolaris");
                    break;
            }
            Room room = PhotonNetwork.room;
            room.open = false;
            setPositions = true;
        }
        else
        {
            if (PhotonNetwork.connected && PhotonNetwork.room != null && hNetwork.playersInRoom() == 4 && !setPositions)
            {
                setPlayers();
                photonView.RPC("sendPlayerInfo", PhotonTargets.Others, CurrentPlayer.Alias, CurrentPlayer.Guild.Name, CurrentPlayer.MaxHealth, Altar.AltarHealth, CurrentPlayer.Level);
                setPositions = true;
            }
        }
        if (PhotonNetwork.room == null)
        {
            setPositions = false;
        }

        if (CurrentPlayer != null)
        {
            CurrentPlayer.Update();
            CurrentPlayer.BuffManager.Update();
        }
    }

    /// <summary>
    /// Needed to set up the base path that all profiles are stored under.
    /// </summary>
    public static void Initialize()
    {
        Directory.CreateDirectory(Player.PlayerFolderPath);
    }

    private List<Player> LoadPlayers()
    {
        Debug.Log(Player.PlayerFolderPath);

        List<Player> characters = new List<Player>();
        foreach (string folder in Directory.GetDirectories(Player.PlayerFolderPath))
        {
            string playerName = new DirectoryInfo(folder).Name;
            Player toLoad = new Player(playerName);
            characters.Add(toLoad);
        }


        return characters;
    }

    private void OnGUI()
    {
        GUI.skin = hexSkin;
        screenManager.Draw();
    }

    public void setPlayers()
    {
        if (PhotonNetwork.isMasterClient)
        {
            arenaPosition = 0;
            PhotonPlayer[] players = PhotonNetwork.otherPlayers;
            for (int i = 0; i < players.Length; i++)
            {
                photonView.RPC("setPosition", PhotonTargets.Others, players[i].name, arenaPosition++);
            }
            hNetwork.startGame();
            Room room = PhotonNetwork.room;
            room.open = false;
        }
    }

    public void giveDamage(string source, string destination, int damage)
    {
        photonView.RPC("sendDamageReport", PhotonTargets.All, source, destination, damage);
        Debug.Log("Damage detected. Source: " + source + " | Destination: " + destination + " | Damage: " + damage);
    }
    public void networkHeal(string actor, int hp)
    {
        Debug.Log("Heal action detected by: " + actor);
        photonView.RPC("sendHealingReport", PhotonTargets.All, actor, hp);      
    }
    public void giveEffect(string actor, string target, int peffectid, double spellpower)
    {
        photonView.RPC("sendEffect", PhotonTargets.All, target, peffectid, spellpower);
        Debug.Log(actor + " gives effect " + peffectid + " to target");
    }

    public void eliminate(string player)
    {
        photonView.RPC("sendEliminationReport", PhotonTargets.All, player);
        Debug.Log(player + " has been eliminated");
    }

    public Boolean isLastManStanding(String player)
    {
        // If we are in debug mode, just stay in the game.
        if (DEBUG) { return false; }
        if (Opponents.Count == 0) { return true; }

      //  if (photonView.RPC("sendIsEliminatedReport", PhotonTargets.Others, player)) { return true; }
      //  Debug.Log("Not the last man standing -- " + Opponents.Count + " players left");
        return false;
    }

    public void giveKill(string killer) {
        photonView.RPC("sendKillReport", PhotonTargets.All, killer);
        Debug.Log("Kill detected. Killer: " + killer);
    }

    public void altarDown()
    {
        LastLife = true;
    }

    public void giveCitadelDamage(string player, int viewID, int damage, string attackingPlayer, IElement element)
    {
        photonView.RPC("sendCitadelDamage", PhotonTargets.All, player, viewID, damage, attackingPlayer, DefaultElement.Serialize(element));
    }

    public void respawned()
    {
        photonView.RPC("sendHealthUpdate", PhotonTargets.All, CurrentPlayer.Alias, CurrentPlayer.CurrentHealth);
    }

    public void sendAltarHealthUpdate(int newHealth)
    {
        AltarHealth = newHealth;
        photonView.RPC("sendAltarUpdate", PhotonTargets.Others, CurrentPlayer.Alias, newHealth);
    }

    public void SyncIceBeamPosition(string actor, Vector3 position)
    {
        photonView.RPC("sendIceBeamInfo", PhotonTargets.Others, actor, position);
    }

    public void RemoveEffect(string target, int effectId)
    {
        photonView.RPC("removeEffect", PhotonTargets.All, target, effectId);
    }

    [RPC]
    private void sendAltarUpdate(string alias, int newHealth)
    {
        if (alias != CurrentPlayer.Alias)
        {
            Opponents[alias].CurAltarHP = newHealth;
            return;
        }
    }

    [RPC]
    private void sendPlayerInfo(string alias, string myClass, int maxHP, int altarHP, int level)
    {
        if (alias != CurrentPlayer.Alias)
        {
            Debug.Log(alias + " " + myClass);
            Opponents.Add(alias, new OpponentData(myClass, maxHP, altarHP, level));
        }
    }

    [RPC]
    private void sendEffect(string target, int effectIdentifier, double spellpower)
    {
        Debug.Log("I see an effect incoming. It is targeted at " + target + ". I am " + CurrentPlayer.Alias);
        if (target == CurrentPlayer.Alias)
        {
            PEffect p = PEffect.Deserialize(effectIdentifier, spellpower);
            if (p is Assets.Buffs.Debuff)
            {
                CurrentPlayer.BuffManager.AddDebuff(p as Assets.Buffs.Debuff);
            }
            else
            {
                CurrentPlayer.BuffManager.AddBuff(p as Assets.Buffs.Buff);
            }
            //else it's a buff, and until we add particle effects, we don't give a rats ass        
        }
        else
        {
            PEffect p = PEffect.Deserialize(effectIdentifier, spellpower);
            Opponents[target].Buffs.Add(p.GetType(), p);
        }
    }

    [RPC]
    private void removeEffect(string target, int effectIdentifier)
    {
        if (target == CurrentPlayer.Alias)
        {
            CurrentPlayer.BuffManager.RemoveEffect(PEffect.Deserialize(effectIdentifier, 0.0));
        }
        else
        {
            Opponents[target].Buffs.Remove(PEffect.Deserialize(effectIdentifier, 0.0).GetType());
        }
    }

    [RPC]
    private void setPosition(string player, int position)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.playerName == player)
                arenaPosition = position;
        }
    }

    [RPC]
    private void sendIceBeamInfo(string owner, Vector3 location)
    {
        if (owner != CurrentPlayer.Alias)
        {
            Opponents[owner].IceBeamInfo = location;
        }
    }

    [RPC]
    private void sendDamageReport(string source, string destination, int damage)
    {
        Debug.Log(string.Format("{0} sent damage {1} to {2}", source, damage, destination));

        if (destination == this.CurrentPlayer.Alias)
        {
            Debug.Log("And that is me (" + this.CurrentPlayer.Alias + ")");
            int hp = this.CurrentPlayer.TakeDamage(damage, source);
            photonView.RPC("sendHealthUpdate", PhotonTargets.All, destination, hp);
        }
        else
        {
            Debug.Log("But that is not me (I am " + this.CurrentPlayer.Alias + ")");
        }
    }

    [RPC]
    private void sendHealingReport(string actor, int healthToHeal)
    {
        Debug.Log(string.Format("{0} healed {0} for {1}", actor, healthToHeal));
        if (actor == CurrentPlayer.Alias)
        {
            Debug.Log("I, " + actor + " am the target of this heal");
            int hp = CurrentPlayer.Heal(healthToHeal);
            photonView.RPC("sendHealthUpdate", PhotonTargets.All, actor, hp);
        }
        else
        {
            Debug.Log("Good for " + actor + ", but I am " + CurrentPlayer.Alias);
        }
    }

    [RPC]
    private void sendEliminationReport(string player)
    {
        for (int i = 0; i < Opponents.Count; i++)
        {
            Opponents.Remove(player);
        }

        if (player == this.CurrentPlayer.Alias)
        {
            this.CurrentPlayer.Eliminate(Opponents.Count + 1);
        }
    }

    [RPC]
    private void sendKillReport(string killer)
    {
        if (killer == this.CurrentPlayer.Alias)
        {
            this.CurrentPlayer.PlayerKilled();
        }
    }

    [RPC]
    private void sendCitadelDamage(string player, int viewID, int damage, string attackingPlayer, int element)
    {
        if (player == this.CurrentPlayer.Alias)
        {
            Blocks[viewID].Damage = damage;
            Blocks[viewID].AttackingPlayer = attackingPlayer;
            Blocks[viewID].Element = DefaultElement.Deserialize(element);
        }
    }

    [RPC]
    private void sendHealthUpdate(string player, int newHealth)
    {
        if (player != CurrentPlayer.Alias)
        {
            if (Opponents.ContainsKey(player))
                Opponents[player].CurHP = newHealth;
        }
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        for (int i = 0; i < Opponents.Count; i++)
        {
            Opponents.Remove(player.name);
        }
    }

    public void Quit()
    {
        if (hNetwork != null)
        {
            hNetwork.disconnect();
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
            Blocks = null;
        }
        CurrentPlayer.BuffManager.ClearAll();
        Opponents.Clear();
    }

    void OnConnectedToPhoton()
    {      
        Debug.Log(PhotonNetwork.playerName);
    }

    void OnDisconnectedFromPhoton()
    {
        screenManager.ActiveScreen = new Screen_CharacterSelect();
    }

    /// <summary>
    /// Set the player being selected.
    /// </summary>
    /// <param name="name">Name of the player to select.</param>
    public void SelectPlayer(string name)
    {
        int index = FindPlayer(name);
        CurrentPlayer = this.players[index];
    }

    /// <summary>
    /// Find the index of a player by their name.
    /// </summary>
    /// <param name="name">Name of the player to find.</param>
    /// <returns>Index of the player, or -1 if one could not be found.</returns>
    private int FindPlayer(string name)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].Name.Equals(name))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Delete a player under this profile.
    /// </summary>
    /// <param name="name">Name of the player to be deleted.</param>
    public void DeletePlayer(string name)
    {
        // Find the player to delete
        int index = FindPlayer(name);
        CurrentPlayer = players[index];

        // First, delete the player's Citadels
        Citadel.DeleteAllCitadels();

        // Find the player and remove it from the list of players the profile has.
        players[index].DeleteCharacter();
        players.RemoveAt(index);

        if (CurrentPlayer.Name == name)
        {
            CurrentPlayer = players.FirstOrDefault();
        }
        if (DefaultCharacter == name)
        {
            try
            {
                DefaultCharacter = players.FirstOrDefault().Name;
            }
            catch (NullReferenceException)
            {
                DefaultCharacter = null;
            }
            WriteSettings();
        }
    }

    public void CreatePlayer(string name, IGuild guild)
    {
        Player newPlayer = new Player(name, guild);
        players.Add(newPlayer);
    }
}
