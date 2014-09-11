using UnityEngine;
using System.Collections;

public class Screen_Arena : HScreen
{
	private int _baseWidth;
	private int _baseHeight;
	
	private Texture2D _playerHP;
	private Texture2D _altarHP;
	private Texture2D _scrollBack;
    private Texture2D _settings;
    private Texture2D _scoreboard;
    private Texture2D _button;
	
	private Texture2D _badlander;
	private Texture2D _engineer;
	private Texture2D _necromancer;
	private Texture2D _steampunk;
    private Texture2D _noPlayer;
	
	private Rect _playerHPRect;
	private Rect _altarHPRect;
    private Rect _settingsRect;
    private Rect _scoreboardRect;
	
	private GUISkin _hudSkin;
    private GUISkin _transparentSkin;

    InterfaceTextureSet PrimAbilityTex;
    InterfaceTextureSet SecAbilityTex;

    private bool scoreBoardEnabled = false;
	
	public Screen_Arena ()
		: base ()
	{}

    protected override void StartCritical()
	{
        PrimAbilityTex = new InterfaceTextureSet();
        SecAbilityTex = new InterfaceTextureSet();
		_hudSkin = Resources.Load("skins/HUDSkin") as GUISkin;
        _transparentSkin = Resources.Load("skins/TransparentSkin") as GUISkin;
		
		_baseWidth = Screen.width / 8;
		_baseHeight = Screen.height / 40;
		
		BuildTextures();
		BuildRects();

        Vector2 basePosition = new Vector2(Screen.width * .01f, _baseHeight * 11);
        for (int j = 0; j < 3; j++ )
        {
            int k = j;
            if (j >= manager.CurrentPlayer.PrimaryAbilities.Count)
            {
                PrimAbilityTex.Add("prim"+j, new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(basePosition.x, basePosition.y, AbilityPair.IconDims.x, AbilityPair.IconDims.y)));
                PrimAbilityTex.GetByName("prim" + j).Active = false;
            }
            else
            {
                
                manager.CurrentPlayer.PrimaryAbilities[j].Texture.Position = basePosition;              
                PrimAbilityTex.Add("prim" + j, manager.CurrentPlayer.PrimaryAbilities[j].Texture);
                PrimAbilityTex.GetByName("prim" + j).ClearInteractions();
                PrimAbilityTex.GetByName("prim" + j).AppendToInteractions(() => SelectPrimaryAbility(k));
                if (PrimAbilityTex.Selected == null)
                {
                    SelectPrimaryAbility(j);
                }
            }
            basePosition.y += _baseHeight * 8f;
        }

        basePosition = new Vector2(Screen.width - AbilityPair.IconDims.x - Screen.width * .01f, _baseHeight * 11);
        for (int j = 0; j < 3; j++)
        {
            int k = j;
            if (j >= manager.CurrentPlayer.SecondaryAbilities.Count)
            {
                SecAbilityTex.Add("sec" + j, new InterfaceTexture("UI_Elements/UI_ButtonGeneralTemplate", new Rect(basePosition.x, basePosition.y, AbilityPair.IconDims.x, AbilityPair.IconDims.y)));
                SecAbilityTex.GetByName("sec" + j).Active = false;
            }
            else
            {
                manager.CurrentPlayer.SecondaryAbilities[j].Texture.Position = basePosition;
                SecAbilityTex.Add("sec" + j, manager.CurrentPlayer.SecondaryAbilities[j].Texture);
                SecAbilityTex.GetByName("sec" + j).ClearInteractions();
                SecAbilityTex.GetByName("sec" + j).AppendToInteractions(() => SelectSecondaryAbility(k));
                if (SecAbilityTex.Selected == null)
                {
                    SelectSecondaryAbility(j);
                }
            }
            basePosition.y += _baseHeight * 8f;
        }      
	}

    private void SelectPrimaryAbility(int j)
    {
        PrimAbilityTex.Selected = PrimAbilityTex.GetByName("prim" + j);
        manager.CurrentPlayer.ActivePrimary = j;
    }
    private void SelectSecondaryAbility(int j)
    {
        SecAbilityTex.Selected = SecAbilityTex.GetByName("sec" + j);
        manager.CurrentPlayer.ActiveSecondary = j;
    }
    protected override void UpdateCritical()
	{
		if(PhotonNetwork.room == null)
		{
			Application.LoadLevel ("Main");
			base.manager.screenManager.ActiveScreen = new Screen_Lobby ();
		}
        SecAbilityTex.Update();
        PrimAbilityTex.Update();
	}

    protected override void DrawCritical()
	{
		//Draw this player hp and altar health
		GUITools.progressBar(_playerHP, _scrollBack, _scrollBack, 5, _playerHPRect, Mathf.Max((float)manager.CurrentPlayer.CurrentHealth / (float)manager.CurrentPlayer.MaxHealth, 0));
		GUITools.progressBar(_altarHP, _scrollBack, _scrollBack, 5, _altarHPRect, Mathf.Max((float)manager.AltarHealth / (float)Assets.CitadelBuilder.Altar.AltarHealth, 0));

        DrawScoreboardButton();
		DrawOpponentInfo();

        SecAbilityTex.Draw();
        PrimAbilityTex.Draw();


	}
	
	override protected void OnButtonPressed (string button)
	{
		switch (button)
		{
		case "Back":
            base.manager.screenManager.OverlayScreen = null;
            PhotonNetwork.LeaveRoom();
			Application.LoadLevel ("Main");
			break;
		}
	}
	
	private void BuildTextures()
	{
		_badlander = Resources.Load("CloseUps/BadlanderCloseUp") as Texture2D;
		_steampunk = Resources.Load("CloseUps/SteampunkCloseUp") as Texture2D;
		_necromancer = Resources.Load("CloseUps/NecromancerCloseUp") as Texture2D;
		_engineer = Resources.Load("CloseUps/EngineerCloseUp") as Texture2D;
        _scoreboard = Resources.Load("UI_Elements/Endgame/UI_ScoreboardButton") as Texture2D;
        _noPlayer = Resources.Load("CloseUps/NoPlayerCloseUp") as Texture2D;
        _button = Resources.Load("UI_Elements/UI_ButtonGeneralTemplate") as Texture2D;
		
		_playerHP = new Texture2D(1, 1);
		_playerHP.SetPixel(0, 0, Color.green);
		_playerHP.Apply();
		
		_altarHP = new Texture2D(1, 1);
		_altarHP.SetPixel(0, 0, Color.yellow);
		_altarHP.Apply();
		
		_scrollBack = new Texture2D(1, 1);
		_scrollBack.SetPixel(0, 0, Color.black);
		_scrollBack.Apply();
	}
	
	private void BuildRects()
	{
		_playerHPRect = new Rect(_baseWidth, Screen.height - _baseHeight*3, _baseWidth*6, _baseHeight);
		_altarHPRect = new Rect(_baseWidth, Screen.height - _baseHeight*2, _baseWidth*6, _baseHeight);
        _scoreboardRect = new Rect(0.0f, _baseHeight * 0.05f, _scoreboard.width / 2, _scoreboard.height / 2);
	}
	
	private void DrawOpponentInfo()
	{
		GUISkin old = GUI.skin;
        if (_hudSkin != null)
        {
            GUI.skin = _hudSkin;
        }
        int i = 0;
		foreach (var opponent in manager.Opponents)
		{
			GUI.Label(new Rect(_baseWidth*i*2+_baseWidth, _baseHeight, 50, 25), opponent.Value.Level.ToString());

            switch (opponent.Value.Class)
			{
			case "Steampunk":
				if(_steampunk != null)
					GUI.DrawTexture(new Rect(_baseWidth*i*2+_baseWidth, _baseHeight, _baseWidth / 2, _baseWidth / 2), _steampunk);
				break;
			case "Engineer":
				if(_engineer != null)
					GUI.DrawTexture(new Rect(_baseWidth*i*2+_baseWidth, _baseHeight, _baseWidth / 2, _baseWidth / 2), _engineer);
				break;
			case "Necromancer":
				if(_necromancer != null)
					GUI.DrawTexture(new Rect(_baseWidth*i*2+_baseWidth, _baseHeight, _baseWidth / 2, _baseWidth / 2), _necromancer);
				break;
			case "Badlander":
				if(_badlander != null)
					GUI.DrawTexture(new Rect(_baseWidth*i*2+_baseWidth, _baseHeight, _baseWidth / 2, _baseWidth / 2), _badlander);
				break;
			}

            GUI.Label(new Rect(_baseWidth * (2 * i + 1.5f) + 5, _baseHeight * 4 - 25, _baseWidth, 25), opponent.Key);
			
			GUITools.progressBar(_playerHP, _scrollBack, _scrollBack, 2, 
				new Rect(_baseWidth*(2*i + 1.5f), _baseHeight*4, _baseWidth*0.8f, _baseHeight),
                Mathf.Max((float)opponent.Value.CurHP / (float)opponent.Value.MaxHP, 0));
			
			GUITools.progressBar(_altarHP, _scrollBack, _scrollBack, 2,
				new Rect(_baseWidth*(2*i + 1.5f), _baseHeight*5, _baseWidth*0.8f, _baseHeight),
                Mathf.Max((float)opponent.Value.CurAltarHP / (float)opponent.Value.MaxAltarHP, 0));
            i++;
		}

        while (i < 3)
        {
            GUI.Label(new Rect(_baseWidth * (2 * i + 1.5f) + 5, _baseHeight * 4 - 25, _baseWidth, 25), "Eliminated");

            GUI.DrawTexture(new Rect(_baseWidth * i * 2 + _baseWidth, _baseHeight, _baseWidth / 2, _baseWidth / 2), _noPlayer);
            GUITools.progressBar(_playerHP, _scrollBack, _scrollBack, 2,
                new Rect(_baseWidth * (2 * i + 1.5f), _baseHeight * 4, _baseWidth * 0.8f, _baseHeight), 0);

            GUITools.progressBar(_altarHP, _scrollBack, _scrollBack, 2,
                new Rect(_baseWidth * (2 * i + 1.5f), _baseHeight * 5, _baseWidth * 0.8f, _baseHeight), 0);
            i++;
        }

		GUI.skin = old;
	}
    
    private void DrawScoreboardButton()
    {
        // Settings button pressed
        GUISkin old = GUI.skin;
        GUI.skin = _transparentSkin;
        GUI.DrawTexture(_scoreboardRect, _scoreboard);
        if (GUIButtonTexture2D(_scoreboardRect, _scoreboard))
        {
            Debug.Log("Scoreboard button clicked");

            // If scoreboard isn't enabled, enable it.
            if (!scoreBoardEnabled)
            {
                Debug.Log("Setting overlay screen to quit overlay");
                base.manager.screenManager.OverlayScreen = new Screen_QuitOverlay();
               // manager.screenManager.OverlayScreen = new Screen_MatchEndOverlay();
                scoreBoardEnabled = true;
            }
            else
            {
                base.manager.screenManager.OverlayScreen = null;
                scoreBoardEnabled = false;
            }
        }
        GUI.skin = old;
    }
}
