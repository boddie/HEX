using UnityEngine;
using System.Collections;

public class Dropdown
{
	private Texture dropdown;
	private Texture downArrow;
	private float dropdownWidth;
	private float dropdownHeight;
	private bool moveMouse = false;
	private Rect dropRect;
	private Rect arrowRect;
	private Vector2 mouse = Vector2.zero;
	
	public bool Dropped 
	{ 
		get 
		{  
			return (dropRect.height > Screen.height*0.75f) ? true : false;
		} 
	}
	
	public Dropdown () 
	{
		dropdown = Resources.Load("Textures/Black") as Texture;
		downArrow = Resources.Load("UI_Elements/DropDown/downArrow") as Texture;
		dropdownWidth = Screen.width;
		dropdownHeight = Screen.height * 0.1f;
		dropRect = new Rect(0, 0, dropdownWidth, dropdownHeight);
		arrowRect = new Rect(Screen.width / 2 - 100*Screen.width/1366, dropdownHeight - Screen.height*0.07f - 5, 200*Screen.width/1366, Screen.height*0.07f);
	}
	
	public bool ArrowClicked()
	{
		if(Input.GetKey(KeyCode.Mouse0) && arrowRect.Contains(mouse))
			return true;
		return false;
	}

	public void Update () 
	{

		if (Input.GetKey(KeyCode.Mouse0) && arrowRect.Contains(mouse))
		{
			moveMouse = true;
		}
		if(moveMouse && Input.GetKey(KeyCode.Mouse0))
		{
			dropRect = new Rect(0, 0, dropdownWidth, mouse.y);
			arrowRect = new Rect(Screen.width / 2 - 100*Screen.width/1366, dropRect.height - Screen.height*0.07f - 5, 200*Screen.width/1366, Screen.height*0.07f);
		}
		else
		{
			moveMouse = false;
			dropRect = new Rect(0, 0, dropdownWidth, dropdownHeight);
			arrowRect = new Rect(Screen.width / 2 - 100*Screen.width/1366, dropdownHeight - Screen.height*0.07f - 5, 200*Screen.width/1366, Screen.height*0.07f);
		}
	}
	
	public void Draw ()
	{
		mouse = Event.current.mousePosition;
		if(dropdown != null)
		{
			GUI.DrawTexture(dropRect, dropdown);
		}
		if(downArrow != null)
		{
            Color c = GUI.color;
            GUI.color = Color.Lerp(Color.clear, Color.magenta, Time.time);
			GUI.DrawTexture(arrowRect, downArrow);
            GUI.color = c;
		}
	}
}
