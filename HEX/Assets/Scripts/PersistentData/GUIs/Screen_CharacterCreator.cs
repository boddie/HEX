using UnityEngine;
using Assets;
using System.Collections.Generic;

public class Screen_CharacterCreator : HScreen
{
    private string _playerName;

    private Rect _text;

    string selected = "none";

    InterfaceTextureSet Buttons;

    Vector3 lerpPosition;
    Quaternion lerpRotation;

    Vector3 initPosition;
    Quaternion initRotation;

    private Animator _steampunkAnimator;
    private Animator _necromancerAnimator;
    private Animator _badlanderAnimator;
    private Animator _engineerAnimator;

    //private Quaternion _necroRot;
    //private Quaternion _steamRot;

    public Screen_CharacterCreator()
        : base()
    {
        _playerName = "";
    }

    Dictionary<string, GameObject> Cameras;
    private bool init;
    protected override void StartCritical()
    {
        float w = _standardWidth;
        float h = _standardHeight;
        Debug.Log(System.IO.Directory.GetCurrentDirectory());
        string engineerDescription = Resources.Load<TextAsset>("UI_Elements/story/engineerdescription").text;
        string steampunksDescription = Resources.Load<TextAsset>("UI_Elements/story/steampunksdescription").text;
        string necromancerDescription = Resources.Load<TextAsset>("UI_Elements/story/necromancerdescription").text;
        string badlandersDescription = Resources.Load<TextAsset>("UI_Elements/story/badlandersdescription").text;
        string solarisDescription = Resources.Load<TextAsset>("UI_Elements/story/solarisdescription").text;
        string badlandsDescription = Resources.Load<TextAsset>("UI_Elements/story/badlandsdescription").text;
        string sarvikDescription = Resources.Load<TextAsset>("UI_Elements/story/sarvikdescription").text;
        string phesusDescription = Resources.Load<TextAsset>("UI_Elements/story/phesusdescription").text;
        Buttons = new InterfaceTextureSet();
        Buttons.Add("badlander", new InterfaceTexture("UI_Elements/CharacterCreation/UI_LanderBackdrop", new Rect(w * .05f, h * .1f, 2 * w, h), "BADLANDER", delegate() { if (selected == "badlander") { DeselectAll(); ; } else { SelectBadlander(); } }));
        Buttons.Add("engineer", new InterfaceTexture("UI_Elements/CharacterCreation/UI_EngBackdrop", new Rect(w * .05f, h * 1.1f, 2 * w, h), "ENGINEER", delegate() { if (selected == "engineer") { DeselectAll(); ; } else { SelectEngineer(); } }));
        Buttons.Add("necromancer", new InterfaceTexture("UI_Elements/CharacterCreation/UI_NecroBacking", new Rect(.05f * w, 2.1f * h, 2 * w, h), "NECROMANCER", delegate() { if (selected == "necromancer") { DeselectAll(); ; } else { SelectNecromancer(); } }));
        Buttons.Add("steampunk", new InterfaceTexture("UI_Elements/CharacterCreation/UI_SteamBacking", new Rect(.05f * w, 3.1f * h, 2 * w, h), "STEAMPUNK", delegate() { if (selected == "steampunk") { DeselectAll(); } else { SelectSteampunk(); } }));
        Buttons.Add("edesc", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", engineerDescription, new Rect(w * .05f, h * 4.2f, 2 * w, 5.75f * h), new Rect(w * .05f, h * 4.2f, 2 * w, 3 * h)));
        Buttons.Add("sdesc", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", steampunksDescription, new Rect(w * .05f, h * 4.2f, 2 * w, 6 * h), new Rect(w * .05f, h * 4.2f, 2 * w, 3 * h)));
        Buttons.Add("bdesc", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", badlandersDescription, new Rect(w * .05f, h * 4.2f, 2 * w, 5.25f * h), new Rect(w * .05f, h * 4.2f, 2 * w, 3 * h)));
        Buttons.Add("ndesc", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", necromancerDescription, new Rect(w * .05f, h * 4.2f, 2 * w, 5.75f * h), new Rect(w * .05f, h * 4.2f, 2 * w, 3 * h)));
        Buttons.Add("edesc2", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", solarisDescription, new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 4f * h), new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 3 * h)));
        Buttons.Add("sdesc2", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", phesusDescription, new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 4.5f * h), new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 3 * h)));
        Buttons.Add("bdesc2", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", badlandsDescription, new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 3f * h), new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 3 * h)));
        Buttons.Add("ndesc2", new ScrollingInterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", sarvikDescription, new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 5 * h), new Rect(w * 6.45f, h * 4.2f, 1.5f * w, 3 * h)));
        Buttons.GetByName("edesc").Visible = false;
        Buttons.GetByName("sdesc").Visible = false;
        Buttons.GetByName("bdesc").Visible = false;
        Buttons.GetByName("ndesc").Visible = false;
        Buttons.GetByName("edesc2").Visible = false;
        Buttons.GetByName("sdesc2").Visible = false;
        Buttons.GetByName("bdesc2").Visible = false;
        Buttons.GetByName("ndesc2").Visible = false;

        Buttons.Add(new InterfaceTexture("UI_Elements/UI_CreateButton", new Rect(w * 6.45f, 1.7f * h, 1.5f*w, 1.5f*h), this.Create));
        Buttons.Add(new InterfaceTexture("UI_Elements/UI_CancelButton", new Rect(w * 6.45f, .1f * h, 1.5f*w, 1.5f*h), this.Cancel));
        _text =                                                         new Rect(w * 6.45f, 3.3f * h, 1.5f*w, 40);


        Cameras = new Dictionary<string, GameObject>();

        init = false;
    }

    protected override void UpdateCritical()
    {      
        if (Application.loadedLevelName == "CharCreator" && !init)
        {
            init = true;
            Cameras.Add("steampunk", GameObject.Find("SteampunkCamera"));
            Cameras.Add("badlander", GameObject.Find("BadlanderCamera"));
            Cameras.Add("engineer", GameObject.Find("EngineerCamera"));
            Cameras.Add("necromancer", GameObject.Find("NecromancerCamera"));

            _steampunkAnimator = GameObject.Find("SteampunkBase").GetComponent<Animator>();
            _necromancerAnimator = GameObject.Find("NecromancerBase").GetComponent<Animator>();
            _engineerAnimator = GameObject.Find("EngineerBase").GetComponent<Animator>();
            _badlanderAnimator = GameObject.Find("BadlanderBase").GetComponent<Animator>();

            initPosition = Camera.main.transform.position;
            initRotation = Camera.main.transform.rotation;
            DeselectAll();
        }
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, lerpPosition, Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, lerpRotation, Time.deltaTime);
        Buttons.Update();
    }

    protected override void DrawCritical()
    {
        int backup = GUI.skin.label.fontSize;
        GUI.skin.label.fontSize = 20;
        Color backupc = GUI.contentColor;
        GUI.contentColor = Colour.Get(0, 0, 0);
        Buttons.Draw();
        GUI.contentColor = backupc;
        GUI.skin.label.fontSize = backup;
        _playerName = GUI.TextField(_text, _playerName, 16);
    }
    public void Create()
    {
        // Make sure the player has a name.
        if (_playerName != string.Empty)
        {
            IGuild playerGuild = null;
            //Genders playerGender = Genders.Male; // Todo: think about the chicklets

            // Determine which guild the player is associated with.
            switch (selected)
            {
                case "steampunk":
                    playerGuild = new Steampunk();
                    break;
                case "engineer":
                    playerGuild = new Engineer();
                    break;
                case "necromancer":
                    playerGuild = new Necromancer();
                    break;
                case "badlander":
                    playerGuild = new Badlander();
                    break;
                default:
                    Debug.Log("You must choose a guild!");
                    return;
            }

            // Create a new player for the given profile, then return to Character Select
            base.manager.CreatePlayer(_playerName, playerGuild);
            base.manager.DefaultCharacter = _playerName;
            base.manager.WriteSettings();
            Application.LoadLevel("Main");
            manager.screenManager.ActiveScreen = new Screen_CharacterSelect(true);
        }
        else
        {
            Debug.Log("Must provide a player name");
        }
    }
    public void Cancel()
    {
        Application.LoadLevel("Main");
        base.manager.screenManager.ActiveScreen = new Screen_CharacterSelect();
    }
    public void SelectSteampunk()
    {
        ResetAnimations();
        _steampunkAnimator.Play(Animator.StringToHash("Base Layer.Pose"));
        selected = "steampunk";
        lerpPosition = Cameras["steampunk"].transform.position;
        lerpRotation = Cameras["steampunk"].transform.rotation;
        Buttons.Selected = Buttons.GetByName("steampunk");
        Buttons.GetByName("edesc").Visible = false;
        Buttons.GetByName("bdesc").Visible = false;
        Buttons.GetByName("ndesc").Visible = false;
        Buttons.GetByName("sdesc").Visible = true;
        Buttons.GetByName("edesc2").Visible = false;
        Buttons.GetByName("bdesc2").Visible = false;
        Buttons.GetByName("ndesc2").Visible = false;
        Buttons.GetByName("sdesc2").Visible = true;
    }
    public void SelectNecromancer()
    {
        ResetAnimations();
        _necromancerAnimator.Play(Animator.StringToHash("Base Layer.Pose"));
        selected = "necromancer";
        lerpPosition = Cameras["necromancer"].transform.position;
        lerpRotation = Cameras["necromancer"].transform.rotation;
        Buttons.Selected = Buttons.GetByName("necromancer");
        Buttons.GetByName("edesc").Visible = false;
        Buttons.GetByName("bdesc").Visible = false;
        Buttons.GetByName("ndesc").Visible = true;
        Buttons.GetByName("sdesc").Visible = false;
        Buttons.GetByName("edesc2").Visible = false;
        Buttons.GetByName("bdesc2").Visible = false;
        Buttons.GetByName("ndesc2").Visible = true;
        Buttons.GetByName("sdesc2").Visible = false;
    }
    public void SelectBadlander()
    {
        ResetAnimations();
        _badlanderAnimator.Play(Animator.StringToHash("Base Layer.Pose"));
        selected = "badlander";
        lerpPosition = Cameras["badlander"].transform.position;
        lerpRotation = Cameras["badlander"].transform.rotation;
        Buttons.Selected = Buttons.GetByName("badlander");
        Buttons.GetByName("edesc").Visible = false;
        Buttons.GetByName("bdesc").Visible = true;
        Buttons.GetByName("ndesc").Visible = false;
        Buttons.GetByName("sdesc").Visible = false;
        Buttons.GetByName("edesc2").Visible = false;
        Buttons.GetByName("bdesc2").Visible = true;
        Buttons.GetByName("ndesc2").Visible = false;
        Buttons.GetByName("sdesc2").Visible = false;
    }
    public void SelectEngineer()
    {
        ResetAnimations();
        _engineerAnimator.Play(Animator.StringToHash("Base Layer.Pose"));
        selected = "engineer";
        lerpPosition = Cameras["engineer"].transform.position;
        lerpRotation = Cameras["engineer"].transform.rotation;
        Buttons.Selected = Buttons.GetByName("engineer");
        Buttons.GetByName("edesc").Visible = true;
        Buttons.GetByName("bdesc").Visible = false;
        Buttons.GetByName("ndesc").Visible = false;
        Buttons.GetByName("sdesc").Visible = false;
        Buttons.GetByName("edesc2").Visible = true;
        Buttons.GetByName("bdesc2").Visible = false;
        Buttons.GetByName("ndesc2").Visible = false;
        Buttons.GetByName("sdesc2").Visible = false;
        
    }
    public void DeselectAll()
    {
        ResetAnimations();
        selected = "none";
        lerpPosition = initPosition;
        lerpRotation = initRotation;
        Buttons.Selected = null;
        Buttons.GetByName("edesc").Visible = false;
        Buttons.GetByName("bdesc").Visible = false;
        Buttons.GetByName("ndesc").Visible = false;
        Buttons.GetByName("sdesc").Visible = false;
        Buttons.GetByName("edesc2").Visible = false;
        Buttons.GetByName("bdesc2").Visible = false;
        Buttons.GetByName("ndesc2").Visible = false;
        Buttons.GetByName("sdesc2").Visible = false;
    }
    private void ResetAnimations()
    {
        _steampunkAnimator.Play(Animator.StringToHash("Base Layer.attack"));
        _necromancerAnimator.Play(Animator.StringToHash("Base Layer.attack"));
        _badlanderAnimator.Play(Animator.StringToHash("Base Layer.attack"));
        _engineerAnimator.Play(Animator.StringToHash("Base Layer.attack"));
    }
}
