using UnityEngine;
using System.Collections;

public class User : MonoBehaviour 
{
    public ScreenPad pad;
    private float baseWidth;
    private float baseHeight;

	void Start () 
    {
        baseWidth = Screen.width / 8;
        baseHeight = Screen.height / 40;
        pad = new ScreenPad(new Rect(baseWidth, baseHeight * 22, 300, 300));
	}
	
	void Update () 
    {
        //pad.Update();
	}

    void OnGUI()
    {
        //pad.OnGUI();
    }
}
