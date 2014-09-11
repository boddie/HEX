using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager
{
    public HScreen ActiveScreen { get; set; }
    public HScreen OverlayScreen { get; set; }
    public HScreen PreviousScreen { get; set; }
    public HScreen DropDownPreviousScreen { get; set; }
	
	public ScreenManager()
	{
		ActiveScreen = new Screen_CharacterSelect(true);
	}
	
	public void Update ()
	{
		ActiveScreen.Update ();
        if (OverlayScreen != null)
        {
            OverlayScreen.Update();
        }
	}
	
	public void Draw ()
	{
		ActiveScreen.Draw ();
        if (OverlayScreen != null)
        {
            OverlayScreen.Draw();
        }
	}
}
