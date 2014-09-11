using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Screen_FindMatch : HScreen
{
    InterfaceTextureSet textures;
    protected override void StartCritical()
    {
        textures = new InterfaceTextureSet();
        textures.Add("waiting", new InterfaceTexture("UI_Elements/confirmed", new Rect(_standardWidth * 2, _standardHeight * 2, _standardWidth * 4, _standardHeight * 2)));
        textures.Add("back", new InterfaceTexture("UI_Elements/UI_BackButton", new Rect(_standardWidth * 3.5f, _standardHeight * 6f, _standardWidth, _standardHeight), ReturnToLobby));
        PhotonNetwork.automaticallySyncScene = true;
        base.manager.hNetwork.joinRandom();
    }
    private void ReturnToLobby()
    {
        if (!Application.loadedLevelName.Contains("Arena") && !Application.isLoadingLevel && !manager.DEBUG && manager.hNetwork.playersInRoom() < 4)
        {
            manager.screenManager.ActiveScreen = manager.screenManager.PreviousScreen;
            manager.screenManager.PreviousScreen = null;
            PhotonNetwork.automaticallySyncScene = false;
            base.manager.hNetwork.disconnect();
        }
        else
        {
            textures.GetByName("back").Active = false;
        }
    }
    protected override void UpdateCritical()
    {
        if (Application.loadedLevelName == "Arena" || 
            Application.loadedLevelName == "ArenaBadlands" ||
            Application.loadedLevelName == "ArenaSolaris")
        {
            base.manager.screenManager.ActiveScreen = new Screen_Arena();
            base.manager.screenManager.PreviousScreen = new Screen_Lobby();
        }
        textures.Update();
    }
    protected override void DrawCritical()
    {
        textures.Draw();
        if (PhotonNetwork.room == null)
        {
            textures.GetByName("waiting").Text = "Looking for room";
        }
        else
        {
            textures.GetByName("waiting").Text = "Waiting in " + PhotonNetwork.room.name + " for other players...";
        }
    }
}

