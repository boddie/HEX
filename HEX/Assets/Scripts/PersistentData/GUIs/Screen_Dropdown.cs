using UnityEngine;
using System.Collections;

public class Screen_Dropdown : HScreen
{
	private Texture dropdown;
	private Texture upArrow;
	private Rect arrowRect;
	private Rect dropRect;
	
	private Texture Achievs;
	private Texture Builder;
	private Texture Profile;
	private Texture Rankings;
	private Texture Settings;
	private Texture Stories;
	
	private Rect AchievsRect;
	private Rect BuilderRect;
	private Rect ProfileRect;
	private Rect RankingsRect;
	private Rect SettingsRect;
	private Rect StoriesRect;

    private Rect AchievesLabel;
    private Rect BuilderLabel;
    private Rect ProfileLabel;
    private Rect RankingsLabel;
    private Rect SettingsLabel;
    private Rect StoriesLabel;
	
	private float blockWidth;
	private float blockHeight;

	public Screen_Dropdown ()
		: base ()
	{
		dropdown = Resources.Load("Textures/Black") as Texture;
		upArrow = Resources.Load("UI_Elements/DropDown/upArrow") as Texture;
		dropRect = new Rect(0, 0, Screen.width, Screen.height);
		arrowRect = new Rect(Screen.width / 2 - 100*Screen.width/1366, Screen.height - Screen.height*0.07f - 5, 200*Screen.width/1366, Screen.height*0.07f);
		
		Achievs = Resources.Load ("UI_Elements/DropDown/UI_Achievements") as Texture;
		Builder = Resources.Load ("UI_Elements/DropDown/UI_Builder") as Texture;
        Profile = Resources.Load("UI_Elements/DropDown/UI_Profile") as Texture;
        Rankings = Resources.Load("UI_Elements/DropDown/UI_Rankings") as Texture;
        Settings = Resources.Load("UI_Elements/DropDown/UI_Settings") as Texture;
        Stories = Resources.Load("UI_Elements/DropDown/UI_Stories") as Texture;
		
		blockWidth = Screen.width / 7f;
		blockHeight = Screen.height / 5f;
		
		AchievsRect = new Rect(blockWidth, blockHeight, blockWidth, blockHeight);
		BuilderRect = new Rect(blockWidth*3, blockHeight, blockWidth, blockHeight);
		ProfileRect = new Rect(blockWidth*5, blockHeight, blockWidth, blockHeight);
		RankingsRect = new Rect(blockWidth, blockHeight*3, blockWidth, blockHeight);
		SettingsRect = new Rect(blockWidth*3, blockHeight*3, blockWidth, blockHeight);
		StoriesRect = new Rect(blockWidth*5, blockHeight*3, blockWidth, blockHeight);

        AchievesLabel = new Rect(AchievsRect.x, AchievsRect.y + AchievsRect.height, AchievsRect.width, 40);
        BuilderLabel = new Rect(BuilderRect.x, BuilderRect.y + BuilderRect.height, BuilderRect.width, 40);
        ProfileLabel = new Rect(ProfileRect.x, ProfileRect.y + ProfileRect.height, ProfileRect.width, 40);
        RankingsLabel = new Rect(RankingsRect.x, RankingsRect.y + RankingsRect.height, RankingsRect.width, 40);
        SettingsLabel = new Rect(SettingsRect.x, SettingsRect.y + SettingsRect.height, SettingsRect.width, 40);
        StoriesLabel = new Rect(StoriesRect.x, StoriesRect.y + StoriesRect.height, StoriesRect.width, 40);
	}

    protected override void StartCritical()
	{
		
	}

    protected override void UpdateCritical()
	{
		
	}

    private bool _calcText = false;

    protected override void DrawCritical()
	{
		if(dropdown != null)
		{
			GUI.DrawTexture(dropRect, dropdown);
		}
		if(upArrow != null)
		{
			GUI.DrawTexture(arrowRect, upArrow);
		}
		if(GUI.Button(arrowRect, "", "Label"))
		{
			base.manager.screenManager.ActiveScreen = base.manager.screenManager.DropDownPreviousScreen;
		}

        if (!_calcText)
        {
            _calcText = true;

            Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("ACHIEVEMENTS"));
            AchievesLabel.x += (AchievesLabel.width - textSize.x) / 2;

            textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("BUILDER"));
            BuilderLabel.x += (BuilderLabel.width - textSize.x) / 2;

            textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("CHARACTERS"));
            ProfileLabel.x += (ProfileLabel.width - textSize.x) / 2;

            textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("TALENTS"));
            RankingsLabel.x += (RankingsLabel.width - textSize.x) / 2;

            textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("SETTINGS"));
            SettingsLabel.x += (SettingsLabel.width - textSize.x) / 2;

            textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("LORE"));
            StoriesLabel.x += (StoriesLabel.width - textSize.x) / 2;
        }
		
		if(GUIButtonTexture2D(AchievsRect, Achievs))
		{
			
		}
        GUI.Label(AchievesLabel, "ACHIEVEMENTS");

		if(GUIButtonTexture2D(BuilderRect, Builder))
		{
			base.manager.screenManager.ActiveScreen = new Screen_CitadelSelector();
		}
        GUI.Label(BuilderLabel, "BUILDER");

		if(GUIButtonTexture2D(ProfileRect, Profile))
		{
			base.manager.screenManager.ActiveScreen = new Screen_CharacterSelect();
		}
        GUI.Label(ProfileLabel, "CHARACTERS");

		if(GUIButtonTexture2D(RankingsRect, Rankings)) 
		{
            base.manager.screenManager.ActiveScreen = new Screen_Character();
		}
        GUI.Label(RankingsLabel, "TALENTS");

		if(GUIButtonTexture2D(SettingsRect, Settings))
		{
            base.manager.screenManager.ActiveScreen = new Screen_Settings();
		}
        GUI.Label(SettingsLabel, "SETTINGS");

		if(GUIButtonTexture2D(StoriesRect, Stories))
		{
			
		}
        GUI.Label(StoriesLabel, "LORE");

	}
}
