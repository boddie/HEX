using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Assets.Buffs;
using Assets.CitadelBuilder;

public class Player
{   
    public int[] SelectedTalents { get; private set; }
    public List<AbilityPair> PrimaryAbilities { get; private set; }           // Primary abilities unlocked by the player.
    public List<AbilityPair> SecondaryAbilities { get; private set; }         // Secondary abilities unlocked by the player.
    public UnlockMats? UnlockedMaterial { get; set; } //unlocked material
    public SpireType? UnlockedSpire { get; set; } //the unlocked spire
    public UnlockBlocks? UnlockedBlock { get; set; } //Unlocked Prefab. 

    public PlayerStats TotalStats { get; private set; } // Persistent, total character stats.
    public PlayerStats MatchStats { get; private set; } // Temporary stats for the current round.

    public string LastDamagedBy { get; private set; }

    public IGuild Guild { get; private set; }
    public string Name { get; private set; }
    public string Alias { get; set; }
    public int Rating { get; set; }
    public int DefaultCitadel { get; set; }
    private int _totalExperience;

    public int TotalExperience
    {
        get
        {
            return _totalExperience;
        }
        set
        {
            _totalExperience = value;
        }

    }

    public int DisplayExperience
    {
        get
        {
            //subtract off the experience required from last level...
            return _totalExperience - (int)Math.Exp((Level / 1.5) + 4);
        }
    }
    public int ExperienceForNextlevel
    {
        get
        {
            int nextLevel = Level + 1;
            //subtract off the experience from last level...
            return (int)(Math.Exp((nextLevel / 1.5) + 4) - Math.Exp(((nextLevel-1)/1.5) + 4));
        }
    }
    public int Level
    {
        get 
        { 
        //    Debug.Log("Level: " + Math.Min(10, (int) (1.5*(Math.Log(_totalExperience) - 4))));
        //    Debug.Log("LevelPrecise: " + (1.5 * (Math.Log(_totalExperience) - 4)));
        //    Debug.Log("Experience : " +_totalExperience);
            return Math.Min(10, (int) (1.5*(Math.Log(_totalExperience) - 4))); 
        }
    }
    public int ConstructionPoints
    {
        get { return 1000 + ((Level - 1) * 100); }
    }
   
    
    //LOAD
    public Player(string name) : this()
    {
        Load(name);
    }
    //CREATE
    public Player(string name, IGuild guild)
        : this()
    {
        Guild = guild;
        Name = name;
        Alias = name;
        TotalStats = new PlayerStats(0, 0); // Start with 0 kills and 0 deaths.
        MatchStats = new PlayerStats(0, 0); // Initialize elsewhere
        DefaultCitadel = -1;
        Save();
    }
    //HELPER
    private Player()
    {
        SelectedTalents = new int[10];
        for (int i = 0; i < SelectedTalents.Length; i++)
        {
            SelectedTalents[i] = 3; //3 signifies noselect
        }
        _totalExperience = 4000; //temp, start at level 2 for demo
        Rating = 1000;
        TotalStats = new PlayerStats(0, 0); // Start with 0 kills and 0 deaths.
        MatchStats = new PlayerStats(0, 0); // Initialize elsewhere
        SpellPower = 1.0f;
        SpellDefense = 0.0f;
        MovementSpeed = 1.0f;
        MaxHealth = CurrentHealth = 200;
       // DeathCount = 0;
        BuffManager = new BuffManager();

        PrimaryAbilities = new List<AbilityPair>();
        SecondaryAbilities = new List<AbilityPair>();
    }

    public void Update()
    {
        _curCooldown = Math.Max(0.0, _curCooldown - Time.deltaTime);
        foreach (var ap in PrimaryAbilities)
        {
            ap.Update();
        }
        foreach (var ap in SecondaryAbilities)
        {
            ap.Update();
        }
    }

    //Combat
    private double _gcd = 0.5f; // Global cooldown between player casts.
    private double _curCooldown;     // Amount of time, in seconds, remaining before the player can cast.
    public int ActivePrimary { get; set; }    // Index of the primary ability selected in battle.
    public int ActiveSecondary { get; set; }  // Index of the secondary ability selected in battle.
    private double _spellpower;
    private double _spelldefense;
    private double _movementspeed;
    public double SpellPower
    {
        get
        {
            return _spellpower;
        }
        set
        {
            _spellpower = Math.Round(value, 2);
        }
    }//as a ratio where 1 is default, i.e. 1.2 is 20% more damage
    public double SpellDefense
    {
        get
        {
            return _spelldefense;
        }
        set
        {
            _spelldefense = Math.Round(value, 2);
        }
    }//as a ratio where 1 is default, i.e. 1.2 is 20% more defense (blocks 20% of damage)
    public double MovementSpeed
    {
        get
        {
            return _movementspeed;
        }
        set
        {
            _movementspeed = Math.Round(value, 2);
        }
    }//as a ratio where 1 is default
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public Vector3 Position { get; set; } //ONLY PLAYERSCRIPT SHOULD EVER SET THIS
    public bool Immobilized { get; set; }
    public bool Silenced { get; set; }
    //public int DeathCount { get; private set; } //number of times a player has died in the current match? Total?
    public BuffManager BuffManager { get; private set; }


    public Vector3 ChangePositionTo { get; set; }

    public bool UsePrimaryTapAbility(TapAbilityUseContext context)
    {
        if (_curCooldown > 0)
        {
            return false;
        }
        _curCooldown = _gcd;

        PrimaryAbilities[ActivePrimary].TapAbility.Use(context);
        return true;
    }

    public bool UsePrimarySwipeAbility(SwipeAbilityUseContext context)
    {
        if (_curCooldown > 0)
        {
            return false;
        }
        _curCooldown = _gcd;
        PrimaryAbilities[ActivePrimary].SwipeAbility.Use(context);
        return true;
    }

    public bool UseSecondaryTapAbility(TapAbilityUseContext context)
    {
        if (_curCooldown > 0)
        {
            return false;
        }
        _curCooldown = _gcd;
        SecondaryAbilities[ActiveSecondary].TapAbility.Use(context);
        return true;
    }

    public bool UseSecondarySwipeAbility(SwipeAbilityUseContext context)
    {
        if (_curCooldown > 0)
        {
            return false;
        }
        _curCooldown = _gcd;

        SecondaryAbilities[ActiveSecondary].SwipeAbility.Use(context);

        return true;
    }

    public int TakeDamage(int damage, String source, bool trueDamage = false )
	{
        LastDamagedBy = source;

        if (trueDamage)
        {
            CurrentHealth -= damage;
        }
        else
        {
		    CurrentHealth -= (int)(damage * (2 - SpellDefense)); //this will give the percentage
        }
		return CurrentHealth;
	}
    public int Heal(int restore)
    {
        CurrentHealth = Math.Min(CurrentHealth + restore, MaxHealth);
        return CurrentHealth;
    }

    public void Resurrect()
    {
        CurrentHealth = MaxHealth;
    }

    public void PlayerDied()
    {
        MatchStats.Deaths++;
        Debug.Log(Alias + " has died. Total deaths: " + MatchStats.Deaths);
    }

    public void PlayerKilled()
    {
        MatchStats.Kills++;
        Debug.Log(Alias + " scored a kill. Total kills: " + MatchStats.Kills);
    }

    public void Eliminate(int position)
    {
        Debug.Log("Player eliminated. Finished in position " + position);
        PlayerStats.place place = (PlayerStats.place) position;
        MatchStats.Placement = place;

        if (place != PlayerStats.place.LEAVE)
        {
            TotalStats.Kills += MatchStats.Kills;
            TotalStats.Deaths += MatchStats.Deaths;
        }
    }

    //Persistence
    private const string PlayerFileExtension = ".player";
    public static string HexFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\HEX\\";
    public static string PlayerFolderPath = HexFolderPath + "\\Players\\";
    public const string InfoFileName = "info" + PlayerFileExtension;
    private string CharFilePath
    {
        get { return CharFolderPath + InfoFileName; }
    }
    public string CharFolderPath
    {
        get { return PlayerFolderPath + Name + "\\"; }
    }

    private void Load(string charName)
    {
        Name = charName;
        Debug.Log("Loading Character: " + Name);
        Debug.Log(File.Exists(CharFilePath));

        string[] infoFile = File.ReadAllLines(CharFilePath);
        foreach (var line in infoFile)
        {
            string[] entry = line.Split('=');
            switch (entry[0])
            {
                case "Guild":
                    SetGuild(entry[1]);
                    break;
                case "Experience":
                    _totalExperience = int.Parse(entry[1]);
                    break;
                case "Talents":
                    SetTalents(entry[1]);
                    break;
                case "Rating":
                    Rating = int.Parse(entry[1]);
                    break;
                case "Kills":
                    TotalStats.Kills = int.Parse(entry[1]);
                    break;
                case "Deaths":
                    TotalStats.Deaths = int.Parse(entry[1]);
                    break;
                case "DefaultCitadel":
                    DefaultCitadel = int.Parse(entry[1]);
                    break;
            }
        }
    }
    public void Save()
    {
        Debug.Log("Saving Player: " + Name);

        string[] lines = new string[]
        {
            "Guild=" + Guild.Name,
            "Experience=" + _totalExperience,
            "Rating=" + Rating,
            "Talents=" + TalentString,
            "Kills=" + TotalStats.Kills,
            "Deaths=" + TotalStats.Deaths,
            "DefaultCitadel=" + DefaultCitadel
        };
        Directory.CreateDirectory(CharFolderPath);
        Debug.Log(CharFolderPath);
        DirectoryInfo d = new DirectoryInfo(CharFolderPath);
        File.WriteAllLines(CharFolderPath + InfoFileName, lines);

        Debug.Log("Saved");
    }
    private string TalentString
    {
        get
        {
            string toReturn = "";
            foreach (int talent in SelectedTalents)
            {
                toReturn += talent.ToString();
            }
            return toReturn;
        }
    }
    private void SetTalents(string talentString)
    {
        for (int i = 0; i < talentString.Length; i++)
        {
            SelectedTalents[i] = Convert.ToInt32(""+talentString[i]);
            Debug.Log("string: "+talentString[i]+"; int: "+SelectedTalents[i]);
        }
        int level = Level;
        Debug.Log("Level: " + level);
        for (int i = 0; i < level; i++)
        {
            if (SelectedTalents[i] != 3) //they could've just not chosen yet
            {
                TalentDictionary.Talents[i][SelectedTalents[i]].Unlock(this);
            }
        }
    }
    public bool AllTalentsAllocated()
    {
        int currentlevel = Level;
        for (int i = 0; i < currentlevel; i++)
        {
            if (SelectedTalents[i] == 3)
            {
                return false;
            }
        }
        return true;
    }
    private void SetGuild(string guild)
    {
        switch (guild)
        {
            case "Steampunk":
                Guild = new Steampunk();
                break;
            case "Badlander":
                Guild = new Badlander();
                break;
            case "Necromancer":
                Guild = new Necromancer();
                break;
            case "Engineer":
                Guild = new Engineer();
                break;
        }
    }   
    public void DeleteCharacter()
    {
        Debug.Log("Deleting Player: " + Name);
        File.Delete(CharFilePath);
        Debug.Log("Deleting Citadels");

        Debug.Log("Deleting base player directory: " + CharFolderPath);
        Directory.Delete(CharFolderPath);
    }
}
