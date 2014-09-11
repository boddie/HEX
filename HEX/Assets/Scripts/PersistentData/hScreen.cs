using UnityEngine;
using System.Collections;

public abstract class HScreen : Controls
{
    private bool _init;
	protected PersistentData manager;
    protected float _standardWidth;
    protected float _standardHeight;
	
	public HScreen ()
		: base("MenuControls")
	{
		this.manager = GameObject.Find ("Persistence").GetComponent<PersistentData> ();
        if (keyMap.Count == 0)
        {
            keyMap.Add("Back", new Control(KeyCode.Escape));
            keyMap.Add("Enter", new Control(KeyCode.Return));

            SaveKeyMap();
        }
        _init = false;
	}
	
	public void Update()
	{
		base.UpdateEvents ();
        if (!_init)
        {
            Start();
            _init = true;
        }
        UpdateCritical();
	}

    public void Start()
    {
        _standardWidth = Screen.width / 8;
        _standardHeight = Screen.height / 8;
        StartCritical();
    }
    public void Draw()
    {
        if (!_init)
        {
            return;
        }
        DrawCritical();
    }
    protected abstract void DrawCritical();
    protected abstract void StartCritical();
    protected abstract void UpdateCritical();
   
	
	protected bool GUIButtonTexture2D(Rect rect, Texture image)
	{
		GUI.DrawTexture(rect, image, ScaleMode.StretchToFill);
		return GUI.Button (rect, "");
	}
	
	protected bool GUIToggleTexture2D(Rect rect, bool val, Texture image, string text)
	{
		GUI.DrawTexture(rect, image);
		return GUI.Toggle(rect, val, text, "Button");
	}
}
