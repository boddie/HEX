using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public class Screen_QuitOverlay : HScreen
{
    private Texture _quit;
    private Texture _quitBox;

    private Rect _quitRect;
    private Rect _quitBoxRect;

    private const float alpha = 0.50f;
    // Would like to add:
    //      - An experience bar
    //      - Character portrait with level (new level if applicable)
    //      - Simple match statistics (kills, deaths, altars destroyed.. etc)
    //      - Ability to spectate
    //      - Level up animation/sound (when applicable)

    protected override void StartCritical()
    {
        Debug.Log("Starting Quit Overlay");

        _standardWidth = Screen.width / 8;
        _standardHeight = Screen.height / 8;

        // Textures need to be made
        // _endmatch = Resources.Load("UI_Elements/UI_EndMatch") as Texture;  // To be some picture at the top
        _quit = Resources.Load("UI_Elements/UI_BackButton") as Texture2D;
        _quitBox = Resources.Load("UI_Elements/EndGame/UI_QuitOverlay") as Texture2D;

        _quitRect = new Rect(55f, 83.0f , _quitBox.width / 1.25f, _quitBox.height/2.25f);
        _quitBoxRect = new Rect(22f, 25.0f, _quitBox.width, _quitBox.height);
    }

    protected override void UpdateCritical()
    {
    }

    protected override void DrawCritical()
    {
        Color saveColor = GUI.color;
        GUI.color = new Color(saveColor.r, saveColor.g, saveColor.b, alpha);
        GUI.DrawTexture(_quitRect, _quit);
        GUI.DrawTexture(_quitBoxRect, _quitBox);
        GUI.color = saveColor;

        if (GUIButtonTexture2D(_quitRect, _quit))
        {
            base.manager.Quit();
            LoadResultsScreen();
        }
    }
/*
    private void Quit()
    {
        base.manager.hNetwork.disconnect();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
        base.manager.Blocks = null;
    }
*/

    private void LoadResultsScreen()
    {
        base.manager.screenManager.OverlayScreen = null;
        base.manager.screenManager.ActiveScreen = new Screen_IndividualMatchResults();
        Application.LoadLevel("IndividualMatchResults");
        
    }
}
