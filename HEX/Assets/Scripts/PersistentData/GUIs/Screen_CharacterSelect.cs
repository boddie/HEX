using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Assets;

public class Screen_CharacterSelect : HScreen
{	
	private Vector2 _scrollPos = Vector2.zero;
    private GUISkin _skin;

    InterfaceTextureSet its;
    ScrollingInterfaceTextureSet characters;

    bool _firstTime;
    public Screen_CharacterSelect(bool firstTime = false)
    {
        _firstTime = firstTime;
    }

    protected override void StartCritical()
    {	
		_skin = Resources.Load("skins/HexSkin") as GUISkin;
        characters = new ScrollingInterfaceTextureSet(new Rect(_standardWidth*.5f, _standardHeight * 1.5f, _standardWidth * 3f, _standardHeight * 5f));
        its = new InterfaceTextureSet();
        its.Add(new InterfaceTexture("UI_Elements/CharacterCreation/UI_SelectACharacter", new Rect(_standardWidth * 2, _standardHeight*.25f, _standardWidth * 4, _standardHeight)));       
        its.Add("steampunk", new InterfaceTexture("UI_Elements/CharacterCreation/UI_Steampunk", new Rect(_standardWidth * 4.25f, _standardHeight * 1.5f, _standardWidth*3, _standardHeight * 6.25f)));
        its.Add("necromancer", new InterfaceTexture("UI_Elements/CharacterCreation/UI_Necromancer", new Rect(_standardWidth * 4.25f, _standardHeight * 1.5f, _standardWidth*3, _standardHeight * 6.25f)));
        its.Add("badlander", new InterfaceTexture("UI_Elements/CharacterCreation/UI_Badlander", new Rect(_standardWidth * 4.25f, _standardHeight * 1.5f, _standardWidth*3, _standardHeight * 6.25f)));
        its.Add("engineer", new InterfaceTexture("UI_Elements/CharacterCreation/UI_Engineer", new Rect(_standardWidth * 4.25f, _standardHeight * 1.5f, _standardWidth*3, _standardHeight * 6.25f)));
        its.Add("level", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 5f, _standardHeight * 5.5f, _standardWidth, _standardHeight)));
        its.Add("rating", new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 6f, _standardHeight * 5.5f, _standardWidth, _standardHeight)));
        its.Add(new InterfaceTexture("UI_Elements/UI_EnterButton", new Rect(_standardWidth * 2.5f, _standardHeight * 6.75f, _standardWidth*.9f, _standardHeight), Enter));
        its.Add(new InterfaceTexture("UI_Elements/UI_CreateButton", new Rect(_standardWidth * 1.5f, _standardHeight * 6.75f, _standardWidth*.9f, _standardHeight), CreateCharacter));
        its.Add(new InterfaceTexture("UI_Elements/UI_DeleteButton", new Rect(_standardWidth * .5f, _standardHeight * 6.75f, _standardWidth*.9f, _standardHeight), DeleteCharacter));
        its.GetByName("level").Visible = false;
        its.GetByName("rating").Visible = false;
        its.GetByName("necromancer").Visible = false;
        its.GetByName("engineer").Visible = false;
        its.GetByName("badlander").Visible = false;
        its.GetByName("steampunk").Visible = false;
        BuildPlayerList();
        try
        {
            SelectCharacter(manager.DefaultCharacter);
            if (true && _firstTime) //options flag
            {              
                Enter();
            }
        }
        catch (NullReferenceException)
        {
            CreateCharacter();
        }
        catch (ArgumentNullException)
        {
            CreateCharacter();
        }
    }
    private void BuildPlayerList()
    {
        float offset = 0;
        foreach (Player p in manager.players)
        {
            string playerName = p.Name;
            characters.Add(playerName, new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth*.5f, _standardHeight * 1.75f + offset, _standardWidth * 2f, _standardHeight), playerName, delegate() { SelectCharacter(playerName); }));
            characters.Add(new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(_standardWidth * 2.5f, _standardHeight * 1.75f + offset, _standardWidth, _standardHeight), "Level " + p.Level + "\nRating " + p.Rating));
            offset += _standardHeight;
        }
    }

    protected override void UpdateCritical()
	{
        its.Update();
        characters.Update();
	}

    private void SelectCharacter(string name)
    {
        characters.Selected = characters.GetByName(name);
        
        foreach (var player in manager.players)
        {
            if (player.Name == name)
            {
                manager.SelectPlayer(player.Name);
                if (player.Guild.GetType() == typeof(Necromancer))
                {
                    its.GetByName("necromancer").Visible = true;
                    its.GetByName("badlander").Visible = false;
                    its.GetByName("engineer").Visible = false;
                    its.GetByName("steampunk").Visible = false;
                }
                else if (player.Guild.GetType() == typeof(Badlander))
                {
                    its.GetByName("necromancer").Visible = false;
                    its.GetByName("badlander").Visible = true;
                    its.GetByName("engineer").Visible = false;
                    its.GetByName("steampunk").Visible = false;
                }
                else if (player.Guild.GetType() == typeof(Engineer))
                {
                    its.GetByName("necromancer").Visible = false;
                    its.GetByName("badlander").Visible = false;
                    its.GetByName("engineer").Visible = true;
                    its.GetByName("steampunk").Visible = false;
                }
                else if (player.Guild.GetType() == typeof(Steampunk))
                {
                    its.GetByName("necromancer").Visible = false;
                    its.GetByName("badlander").Visible = false;
                    its.GetByName("engineer").Visible = false;
                    its.GetByName("steampunk").Visible = true;
                }
            }
        }
    }

    private void DeleteCharacter()
    {
        manager.DeletePlayer(manager.CurrentPlayer.Name);
        characters.Clear();
        BuildPlayerList();
        try
        {
            SelectCharacter(manager.players.FirstOrDefault().Name);
        }
        catch (NullReferenceException)
        {
            CreateCharacter();
        }

    }

    private void CreateCharacter()
    {
        Application.LoadLevel("CharCreator");
     //   base.manager.screenManager.PreviousScreen = this;
        base.manager.screenManager.ActiveScreen = new Screen_CharacterCreator();

    }

    private void Enter()
    {
        base.manager.CurrentPlayer.Save();
        Assets.CitadelBuilder.Citadel.LoadCitadels();
        if (base.manager.DEBUG)
        {
            base.manager.hNetwork.connect("1.2");
        }
        else
        {
            base.manager.hNetwork.connect("1.0");
        }
        manager.DefaultCharacter = manager.CurrentPlayer.Name;
        manager.CurrentPlayer.Alias = (manager.hNetwork.setNetworkAlias(manager.CurrentPlayer.Name));
        manager.WriteSettings();
        manager.screenManager.PreviousScreen = this;
        manager.screenManager.ActiveScreen = new Screen_Lobby();
    }
	
    protected override void DrawCritical()
	{
        its.Draw();
        characters.Draw();
	}
}
