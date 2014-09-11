using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Screen_IndividualMatchResults : HScreen
{
    private float timeElapsed = 0;
    private float lastFrameTime = 0;
    private float delay = 0.2f;

    private int _baseWidth;
    private int _baseHeight;

    Player CurrentPlayer;

    #region Consts
    private const int BASE_EXPERIENCE_GAIN = 1000;
    private const int EXPERIENCE_PER_PLAYER_KILL = 50;
    private const int EXPERIENCE_TO_ANIMATE_PER_INTERVAL = 5;
    private const float ANIMATE_INTERVAL = 0.03f; // Animation interval in seconds.
    #endregion

    #region Buttons
    private Texture _continue;
    private Rect _continueRect;
    private Rect _resultsRect;
    #endregion

    #region Text Display
    GameObject _textTest;
    GameObject _textExp;
    GameObject _currentExperience;

    TextMesh _textMeshTest;
    TextMesh _textMeshExp;
    TextMesh _textCurrentExperience;
    #endregion

    #region Character Model
    private GameObject characterBase;
    private GameObject newChar;

    string prefabLocation;
    private Animator characterAnimator;
    #endregion

    #region Progress bar
    private Texture2D _playerXP;
    private Texture2D _scrollBack;
    private Rect _playerXPRect;
    Color progressColor = new Color(0.039f, 0.99f, 0.39f);
    Color backColor = new Color(0.156f, 0.156f, 0.156f);

    private int experienceToGain = 0;
    private int experienceAnimatedSoFar = 0;
    #endregion

    #region Flags
    bool doneAnimating = false;
    bool changesDone = false;
    private Boolean enter = false;
    #endregion

    public Screen_IndividualMatchResults()
        : base()
    { }

    protected override void StartCritical()
    {
        CurrentPlayer = base.manager.CurrentPlayer;

        _standardWidth = Screen.width / 8;
        _standardHeight = Screen.height / 8;

        _continue = Resources.Load("UI_Elements/EndGame/UI_Continue") as Texture2D;
        _continueRect = new Rect(_standardWidth * 4.0f - (_standardWidth / 2), _standardHeight * 6.5f, _standardWidth, _standardHeight);

        SetupProgressBarTextures();

        SetupTextDisplays();

        Debug.Log("Experience towards next level: " + CurrentPlayer.DisplayExperience);
        Debug.Log("Experience until next level: " + CurrentPlayer.ExperienceForNextlevel);
        Debug.Log("Percentage Progress to Next Level :" + Mathf.Max((float)CurrentPlayer.DisplayExperience / (float)CurrentPlayer.ExperienceForNextlevel, 0));

        CalculateExperienceGain();

        Delay();
    }

    protected override void UpdateCritical()
    {
        if (enter)
        {
            lastFrameTime += Time.deltaTime;

            if (lastFrameTime > delay)
            {
                base.manager.CurrentPlayer.Save();
                base.manager.screenManager.ActiveScreen = new Screen_Lobby();
                Application.LoadLevel("Main");
            }
        }

        _textTest = GameObject.Find("expText");
        _textExp = GameObject.Find("testText");

        _currentExperience = GameObject.Find("currentExperienceText");
        _textCurrentExperience = _currentExperience.GetComponent<TextMesh>();
        _currentExperience.transform.position = new Vector3(-3.10f, 1.114783f, 5.239626f);

        if (newChar == null)
        {
            switch (CurrentPlayer.Guild.Name)
            {
                case "Engineer":
                    newChar = (GameObject)Resources.Load("engineerNoPhys");
                    PlayCharacterAnimation();

                    break;
                case "Necromancer":
                    //Debug.Log("PREFAB LOCATION: " + CurrentPlayer.Guild.Prefab + "NoPhys");
                    newChar = (GameObject)Resources.Load("necromancerNoPhys");
                    PlayCharacterAnimation();
                    break;
                case "Badlander":
                    newChar = (GameObject)Resources.Load("badlanderNoPhys");
                    PlayCharacterAnimation();

                    break;

                case "Steampunk":
                    newChar = (GameObject)Resources.Load("steampunkNoPhys");
                    PlayCharacterAnimation();

                    break;

                default:

                    break;
            }
        }

        if (doneAnimating && !changesDone)
        {
            CurrentPlayer.TotalExperience += experienceToGain;
            changesDone = true;
        }
    }

    protected override void DrawCritical()
    {
        if (doneAnimating)
        {
            GUITools.progressBar(_playerXP, _scrollBack, _scrollBack, 5, _playerXPRect, Mathf.Max((float)CurrentPlayer.DisplayExperience / (float)CurrentPlayer.ExperienceForNextlevel, 0));
            _currentExperience = GameObject.Find("currentExperienceText");
            _textCurrentExperience = _currentExperience.GetComponent<TextMesh>();
            _textCurrentExperience.text = (float)CurrentPlayer.DisplayExperience + "XP / " + (float)CurrentPlayer.ExperienceForNextlevel + "XP";
            _currentExperience.transform.position = new Vector3(-3.10f, 1.114783f, 5.239626f);
        }
        else
        {
            AnimateExperienceGain();
        }

        _textMeshExp = _textExp.GetComponent<TextMesh>();
        _textMeshExp.text = "Experienced Gained: " + experienceToGain;

        String placementString = String.Empty;
        PlayerStats matchStats = manager.CurrentPlayer.MatchStats;

        if (matchStats.Placement != PlayerStats.place.LEAVE && matchStats.Placement != 0)
        {
            switch (matchStats.Placement)
            {
                case PlayerStats.place.FIRST:
                    placementString = "You Placed 1st!";
                    break;
                case PlayerStats.place.SECOND:
                    placementString = "You Placed 2nd!";
                    break;
                case PlayerStats.place.THIRD:
                    placementString = "You placed 3rd!";
                    break;
                case PlayerStats.place.FOURTH:
                    placementString = "You placed 4th!";
                    break;
            }
        }
        else
        {
            placementString = "You left the game early!";
        }

         _textMeshTest = _textTest.GetComponent<TextMesh>();
         _textMeshTest.text = placementString;
        
        GUI.DrawTexture(_continueRect, _continue);

        if (GUIButtonTexture2D(_continueRect, _continue) && doneAnimating)
        {
            // base.manager.screenManager.OverlayScreen = null;
            enter = true;
            //  Quit();
        }

        //   base.manager.hNetwork.disconnect();
    }

    #region Private Methods
    private void ResetAnimations()
    {
        characterAnimator.Play(Animator.StringToHash("Base Layer.attack"));
    }

    private void PlayCharacterAnimation()
    {
        GameObject.Instantiate(newChar);
        characterAnimator = newChar.GetComponent<Animator>();
        ResetAnimations();
        characterAnimator.Play(Animator.StringToHash("Base Layer.Pose"));
    }

    private void Quit()
    {
        if (base.manager.hNetwork != null)
        {
            base.manager.hNetwork.disconnect();
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
            base.manager.Blocks = null;
        }
    }

    private void SetupProgressBarTextures()
    {
        _playerXP = new Texture2D(1, 1);
        _playerXP.SetPixel(0, 0, progressColor);
        _playerXP.Apply();

        _scrollBack = new Texture2D(1, 1);
        _scrollBack.SetPixel(0, 0, backColor);
        _scrollBack.Apply();

        _playerXPRect = new Rect(_standardWidth, Screen.height - _standardHeight * 3, _standardWidth * 6, _standardHeight);
    }

    private void SetupTextDisplays()
    {
        _currentExperience = GameObject.Find("currentExperienceText");
        _textCurrentExperience = _currentExperience.GetComponent<TextMesh>();
        _currentExperience.transform.position = new Vector3(-3.10f, 1.114783f, 5.239626f);

        _textTest = GameObject.Find("expText");
        _textExp = GameObject.Find("testText");
    }

    private void CalculateExperienceGain()
    {
        PlayerStats matchStats = manager.CurrentPlayer.MatchStats;
        Debug.Log("Match stats.placement : " + matchStats.Placement);

        // Only calculate experience gain if the player didn't leave early.
        if (matchStats.Placement != PlayerStats.place.LEAVE && matchStats.Placement != 0)
        {
            experienceToGain = BASE_EXPERIENCE_GAIN / (int)matchStats.Placement;
            experienceToGain += EXPERIENCE_PER_PLAYER_KILL * manager.CurrentPlayer.MatchStats.Kills;
            Debug.Log(manager.CurrentPlayer.Alias + " gained " + experienceToGain + " xp.");
        }
        else
        {
            Debug.Log("The player left the game early. 0 Experience gained");
        }
    }

    private void Delay()
    {
        while (lastFrameTime < .75f)
        {
            lastFrameTime += Time.deltaTime;
        }

        lastFrameTime = 0;
    }

    private void AnimateExperienceGain()
    {
        timeElapsed += Time.deltaTime;

        //   _currentExperience = GameObject.Find("currentExperienceText");
        GUITools.progressBar(_playerXP, _scrollBack, _scrollBack, 5, _playerXPRect, Mathf.Max((float)(CurrentPlayer.DisplayExperience + experienceAnimatedSoFar) / (float)CurrentPlayer.ExperienceForNextlevel, 0));
        _textCurrentExperience.text = (float)(CurrentPlayer.DisplayExperience + experienceAnimatedSoFar) + "XP / " + (float)CurrentPlayer.ExperienceForNextlevel + "XP";
        

        if (timeElapsed > ANIMATE_INTERVAL)
        {
            timeElapsed = 0;
            if (EXPERIENCE_TO_ANIMATE_PER_INTERVAL + experienceAnimatedSoFar > experienceToGain)
            {
                experienceAnimatedSoFar = experienceToGain;
                doneAnimating = true;
            }
            else
            {
                experienceAnimatedSoFar += EXPERIENCE_TO_ANIMATE_PER_INTERVAL;

            }
        }
        else
        {
            // Nothing atm
        }
    }
    #endregion
}