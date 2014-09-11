using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public static class InputHandler
{
    public static Vector2 MousePosition
    {
        get
        {
            return new Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y));
        }
    }
}

public abstract class Controls
{
	public class Control
	{
		public KeyCode button { get; set; }

		public Control(KeyCode button)
		{
			this.button = button;
		}

		public bool CheckKey()
		{
			return Input.GetKeyDown (button);
		}
	}
	
	#region Class Member Variables
	
	protected Dictionary<string, Control> keyMap;
    private string _name;
	
	#endregion
	
	public Controls(string name) 
	{
		keyMap = new Dictionary<string, Control> ();
        _name = name;

        LoadKeyMap();
        
	}
	
    /// <summary>
    /// Load in the keymap for the control set.
    /// </summary>
    private void LoadKeyMap()
    {
        if (File.Exists(_name + ".txt"))
        {
            using (FileStream stream = File.OpenRead(_name + ".txt."))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] entry = reader.ReadLine().Split('=');
                        keyMap.Add(entry[0], new Control((KeyCode)Enum.Parse(typeof(KeyCode), entry[1])));
                    }
                }
            }
        }
    }

    protected void SaveKeyMap()
    {
        using(FileStream stream = File.OpenWrite(_name + ".txt"))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                foreach (KeyValuePair<string, Control> entry in keyMap)
                {
                    writer.WriteLine(entry.Key + "=" + entry.Value.button.ToString("g"));
                }
            }
        }

    }
	
	virtual public void UpdateEvents()
	{
		foreach (KeyValuePair<string, Control> button in keyMap)
		{
			if(button.Value.CheckKey())
			{
				OnButtonPressed(button.Key);
			}
		}
	}
	
	virtual protected void OnButtonPressed(string button) {}
}
