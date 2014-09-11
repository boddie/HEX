using System;
using UnityEngine;

public class Screen_Settings : HScreen
{
    private string[] _selections;
    private int _selection;

    private float qualityLevel;

    private Rect volumeRect;
    private float volumeValue;

    private float baseWidth;
    private float baseHeight;

    public Screen_Settings()
        : base()
    {
        _selections = new string[] { "Key Bindings", "Video"};
        _selection = -1;
    }

    protected override void StartCritical()
    {
        volumeValue = 0;
        qualityLevel = 0;

        baseWidth = Screen.width / 8f;
        baseHeight = Screen.height / 8f;

        SetupRect();
    }

    protected override void UpdateCritical()
    {
        AudioListener.volume = volumeValue * .01f;

        QualitySettings.SetQualityLevel((int) qualityLevel);
    }

    private void SetupRect()
    {
        volumeRect = new Rect(baseHeight * 3, baseHeight * 3, baseWidth*4, baseHeight*4);
    }

    protected override void DrawCritical()
    {
       // volumeValue = GUI.HorizontalSlider(volumeRect, volumeValue, 0.0F, 100.0F);

        GUILayout.Label("Settings");

        GUILayout.Label("Audio settings:");
        GUILayout.Label("Master Volume: " + (int) volumeValue + "%");
        volumeValue = GUILayout.HorizontalSlider(volumeValue, 0, 100.0f);

   //     GUILayout.Label("Key Bindings:");

   //     GUILayout.Label("Video settings:");
        

        for (int i = 0; i < _selections.Length; ++i)
        {
            if (GUILayout.Button(_selections[i]))
            {
                // If already clicked, unselect it, otherwise select it.
                _selection = _selection == i ? -1 : i;
            }
        }

        switch (_selection)
        {
            case 0:

                GUILayout.BeginArea(new Rect(300, 300, 500, 500));

                GUILayout.Label("Key Bindings");

                GUILayout.EndArea();

                break;
            case 1:

                GUILayout.BeginArea(new Rect(300, 300, 500, 500));

                GUILayout.Label("Video settings");

                GUILayout.Label("Graphics: " + qualityLevel + "%");
                qualityLevel = GUILayout.HorizontalSlider(qualityLevel, 0, 100);

                GUILayout.EndArea();

                break;
        }

    }

    protected override void OnButtonPressed(string button)
    {
        switch (button)
        {
            case "Back":
            case "Enter":
                base.manager.settings.Save();
                base.manager.screenManager.ActiveScreen = base.manager.screenManager.PreviousScreen;
                break;
        }
    }
}

