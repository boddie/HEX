using UnityEngine;
using System.Collections;

public class Screen_Scoreboard : HScreen
{

    private bool continueToLobby = false;
    private bool backToSpectate = false;
    private bool gameOver = false;

    private int ping = 0;
    private float timer;

    private Texture _overlay;
    private Texture _continue;
    private Texture _spectate;
    private Texture _title;

    private Rect _continueRect;
    private Rect _spectateRect;
    private Rect _titleRect;
    private Rect _overlayRect;

    private const float alpha = 0.50f;

    public Screen_Scoreboard()
        : base()
    { }

    public Screen_Scoreboard(bool gameOver)
        : base()
    {
        this.gameOver = gameOver;
    }

    // Would like to add:
    //      - An experience bar
    //      - Character portrait with level (new level if applicable)
    //      - Simple match statistics (kills, deaths, altars destroyed.. etc)
    //      - Ability to spectate
    //      - Level up animation/sound (when applicable)

    protected override void StartCritical()
    {
        // Textures need to be made
        // _endmatch = Resources.Load("UI_Elements/UI_EndMatch") as Texture;  // To be some picture at the top
        _continue = Resources.Load("UI_Elements/UI_Continue") as Texture;
        _spectate = Resources.Load("UI_Elements/UI_Spectate") as Texture;
      //  _title = Resources.Load("UI_Elements/UI_GameOver") as Texture;
        _overlay = Resources.Load("UI_Elements/UI_EndMatchOverlay") as Texture;

        _continueRect = new Rect(_standardWidth * 4.25f, _standardHeight * 5.0f, _standardWidth, _standardHeight);
        _spectateRect = new Rect(_standardWidth * 2.75f, _standardHeight * 5.0f, _standardWidth, _standardHeight);
     //   _titleRect = new Rect(_standardWidth * 3.0f, _standardHeight, _title.width, _title.height);
        _overlayRect = new Rect(_standardWidth*2.0f, _standardHeight* 0.5f, _standardWidth*4.0f, _standardHeight*6.25f);

        InitializeRoundStats(); // Calculate experience gained, performance in game (1st, 2nd, etc), and other info.
    }

    protected override void UpdateCritical()
    {
        if (timer < Time.time)
        {
            timer = Time.time + 2;
            ping = PhotonNetwork.GetPing();
        }

        // Not sure if this is correct
        if (Application.loadedLevelName == "Main")
        {
            base.manager.screenManager.ActiveScreen = new Screen_Lobby();
        }

        // To do
    }

    protected override void DrawCritical()
    {

        // If nothing has been selected, continue drawing screen
        if (!continueToLobby && !backToSpectate)
        {
            Color saveColor = GUI.color;
            GUI.color = new Color(saveColor.r, saveColor.g, saveColor.b, alpha);
            GUI.DrawTexture(_overlayRect, _overlay);
            GUI.color = saveColor;


            if (!gameOver)
            {
                GUI.DrawTexture(_continueRect, _continue);
                GUI.DrawTexture(_spectateRect, _spectate);
                //      GUI.DrawTexture(_titleRect, _title);

                if (GUIButtonTexture2D(_continueRect, _continue))
                {
                    continueToLobby = true; // not sure if needed?
                    Application.LoadLevel("IndividualMatchResults");
                    base.manager.screenManager.ActiveScreen = new Screen_IndividualMatchResults();
                }

                if (GUIButtonTexture2D(_spectateRect, _spectate))
                {
                    backToSpectate = true; // not sure if needed?
                    
                    //    base.manager.screenManager.ActiveScreen = new Screen_Arena(); 
                }
            }
            else
            {
                // Draw quit button
            }

            // Draw other stuff.

        }

    }


    public void InitializeRoundStats()
    {

        // To do
    }
}
