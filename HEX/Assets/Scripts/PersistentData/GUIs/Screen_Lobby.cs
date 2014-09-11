using UnityEngine;
using System.Collections;
using Assets.CitadelBuilder;
using Assets;

public class Screen_Lobby : HScreen
{
    private float timeTracker;
    private Dropdown dropdown;

    private int ping = 0;
    private float timer;

    private Texture _toggle;
    private Texture _joinGame;
    private Texture _back;

    InterfaceTextureSet Buttons;

    public Screen_Lobby()
        : base()
    {
        dropdown = new Dropdown();
        ping = PhotonNetwork.GetPing();
        timer = Time.time + 2;
    }

    protected override void StartCritical()
    {
        Buttons = new InterfaceTextureSet();

        Buttons.Add("Notifier", new InterfaceTexture("UI_Elements/Selected", new Rect(_standardWidth * 2, _standardHeight, _standardWidth * 4, _standardHeight)));
        Buttons.Add("Citadel0", new InterfaceTexture("UI_Elements/CharacterCreation/UI_EngBackdrop", new Rect(_standardWidth * 3, _standardHeight * 2.5f, _standardWidth * 2, _standardHeight), CitadelName(0), () => { SelectCitadel(0); }));
        Buttons.Add("Citadel1", new InterfaceTexture("UI_Elements/CharacterCreation/UI_EngBackdrop", new Rect(_standardWidth * 3, _standardHeight * 3.75f, _standardWidth * 2, _standardHeight), CitadelName(1), () => { SelectCitadel(1); }));
        Buttons.Add("Citadel2", new InterfaceTexture("UI_Elements/CharacterCreation/UI_EngBackdrop", new Rect(_standardWidth * 3, _standardHeight * 5.0f, _standardWidth * 2, _standardHeight), CitadelName(2), () => { SelectCitadel(2); }));
        Buttons.Add("Enter", new InterfaceTexture("UI_Elements/UI_EnterButton", new Rect(_standardWidth * 3.5f, _standardHeight * 6.5f, _standardWidth, _standardHeight), Enter));

        int defaultCitadel = manager.CurrentPlayer.DefaultCitadel;
        for (int i = 0; i < manager.Citadels.Length; i++)
        {
            string name = "Citadel"+i;
            if (manager.Citadels[i] == null)
            {
                Buttons.GetByName(name).Active = false;
            }
        }
        Buttons.GetByName("Notifier").Visible = false;
        SelectCitadel(defaultCitadel);
        if (!manager.CurrentPlayer.AllTalentsAllocated())
        {
            Buttons.GetByName("Enter").Active = false;
            Buttons.GetByName("Notifier").Visible = true;
            Buttons.GetByName("Notifier").Text = "Use dropdown above to allocate talents!";
        }
        
    }
    private void Enter()
    {
        manager.screenManager.ActiveScreen = new Screen_FindMatch(); 
        manager.screenManager.PreviousScreen = this;
        manager.CurrentPlayer.DefaultCitadel = manager.CurrentCitadel;
        manager.CurrentPlayer.Save();
    }
    private string CitadelName(int i)
    {
        return manager.Citadels[i] == null ? "<Empty>" : manager.Citadels[i].Name;
    }
    private void SelectCitadel(int i)
    {
        manager.CurrentCitadel = i;
        if (i > -1)
        {
            string selector = "Citadel" + i;
            Buttons.Selected = Buttons.GetByName(selector);
        }
        else
        {
            Buttons.Selected = null;
            Buttons.GetByName("Enter").Active = false;
            Buttons.GetByName("Notifier").Visible = true;
            Buttons.GetByName("Notifier").Text = "Use dropdown above to build a citadel!";
        }
    }

    protected override void UpdateCritical()
    {
        if (timer < Time.time)
        {
            timer = Time.time + 2;
            ping = PhotonNetwork.GetPing();
        }
        if (dropdown.Dropped)
        {
            base.manager.screenManager.DropDownPreviousScreen = new Screen_Lobby();
            base.manager.screenManager.ActiveScreen = new Screen_Dropdown();
        }
        dropdown.Update();
        Buttons.Update();
    }

    protected override void DrawCritical()
    {
        Buttons.Draw();
        dropdown.Draw();
    }
}
