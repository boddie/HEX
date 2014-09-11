using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public class Screen_MatchEndOverlay : HScreen
{
    private Texture _quit;
    private Texture _menu;
    private Texture _spectate;

    private Rect _menuRect;
    private Rect _quitRect;
    private Rect _spectateRect;

    private const float alpha = 0.50f;

    public Screen_MatchEndOverlay()
        : base()
    { }

    // Would like to add:
    //      - An experience bar
    //      - Character portrait with level (new level if applicable)
    //      - Simple match statistics (kills, deaths, altars destroyed.. etc)
    //      - Ability to spectate
    //      - Level up animation/sound (when applicable)

    protected override void StartCritical()
    {
        Debug.Log("Starting Match End Overlay");

        _standardWidth = Screen.width / 8;
        _standardHeight = Screen.height / 8;

        // Textures need to be made
        // _endmatch = Resources.Load("UI_Elements/UI_EndMatch") as Texture;  // To be some picture at the top
        _quit = Resources.Load("UI_Elements/EndGame/UI_Continue") as Texture2D;
        _menu = Resources.Load("UI_Elements/EndGame/UI_EndMatchOverlay") as Texture2D;
        _spectate = Resources.Load("UI_Elements/EndGame/UI_Spectate") as Texture2D;

        _quitRect = new Rect(_standardWidth * 4 - _quit.width/2, _standardHeight * 2.8f, _quit.width, _quit.height);
        _menuRect = new Rect(_standardWidth * 4 - _menu.width/4, _standardHeight/2.5f, _menu.width/2, _menu.height/2);
        _spectateRect = new Rect(_standardWidth * 4 - _spectate.width/2, _standardHeight * 5.25f, _spectate.width, _spectate.height);
    }

    protected override void UpdateCritical()
    {
    }

    protected override void DrawCritical()
    {
        Color saveColor = GUI.color;
        GUI.color = new Color(saveColor.r, saveColor.g, saveColor.b, alpha);
        GUI.DrawTexture(_quitRect, _quit);
        GUI.DrawTexture(_menuRect, _menu);
        GUI.DrawTexture(_spectateRect, _spectate);
        GUI.color = saveColor;

        if (GUIButtonTexture2D(_quitRect, _quit))
        {
            Quit();
            LoadResultsScreen();
        }

        if (GUIButtonTexture2D(_spectateRect, _spectate))
        {
            Spectate();
        }
    }

    private void Quit()
    {
        base.manager.hNetwork.disconnect();
        base.manager.hNetwork.destroyAll();
        base.manager.Blocks = null;
    }

    private void LoadResultsScreen()
    {
        base.manager.screenManager.OverlayScreen = null;
        Application.LoadLevel("IndividualMatchResults");
        base.manager.screenManager.ActiveScreen = new Screen_IndividualMatchResults();
    }

    private void Spectate()
    {
        base.manager.screenManager.OverlayScreen = null;

        // Dafuq happens here
    }
}