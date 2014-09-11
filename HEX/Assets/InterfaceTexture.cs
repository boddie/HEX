using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void Script();
//lets make the simplifying assumption that all textures are made to be 1920x1080, and scaled down otherwise.
public class InterfaceTexture
{
    public bool Visible { get; set; }
    public bool Active { get; set; }
    public string Text { get; set; }
    public bool Selected { get; set; }
    protected Rect selectRect; //only for if a specific part of the button is selected
    public Texture SelectTexture
    {
        get
        {
            return _selectTexture;
        }
        set
        {
            _selectTexture = value;
            selectRect = _interactions.FirstOrDefault()._rec;
        }
    }
    private Texture _selectTexture = null;
    protected Texture _tex;
    private Rect _drawRect;
    public Rect DrawRect
    {
        get
        {
            return _drawRect;
        }
        set
        {
            if (_drawRect == selectRect)
            {
                selectRect = value;
            }
            _drawRect = value;
        }
    }
    public Vector2 Position
    {
        get
        {
            return new Vector2(DrawRect.xMin, DrawRect.yMin);
        }
        set
        {
            DrawRect = new Rect(value.x, value.y, DrawRect.width, DrawRect.height);
        }
    }
    protected List<RecDelPair> _interactions;
    //protected InterfaceTexture(string file, Vector2 pos, params RecDelPair[] interactions)
    //{
    //    _tex = Resources.Load(file) as Texture;
    //    DrawRect = new Rect(pos.x, pos.y, _tex.width, _tex.height);
    //    super(file, DrawRect, interactions);
    //}
    public InterfaceTexture(string file, Rect rec)
    {
        super(file, rec);
    }
    private void super(string file, Rect rec, params RecDelPair[] interactions)
    {
        if (interactions.Length == 0)
        {
            _interactions = new List<RecDelPair>();
            _interactions.Add(new RecDelPair(rec, delegate() { }));
        }
        else
        {
            _interactions = interactions.ToList();
        }
        _tex = Resources.Load(file) as Texture;
        DrawRect = rec;
        SelectTexture = Resources.Load("UI_Elements/selected") as Texture;
        Selected = false;
        Visible = true;
        Active = true;
    }
    //protected InterfaceTexture(string file, Rect rec, params RecDelPair[] interactions)
    //{
    //    super(file, rec, interactions);
    //}
    public InterfaceTexture(string file, Vector2 pos, Script script) 
    {
        _tex = Resources.Load(file) as Texture;
        DrawRect = new Rect(pos.x, pos.y, _tex.width, _tex.height);
        super(file, DrawRect, new RecDelPair(DrawRect, script));
    }
    public InterfaceTexture(string file, Rect rec, string text, Script script) : this(file, rec, script)
    {
        Text = text;           
    }
    //protected InterfaceTexture(string file, Rect rec, string text, params RecDelPair[] script)
    //    : this(file, rec, script)
    //{
    //    Text = text;
    //}
    public InterfaceTexture(string file, Rect rec, string text)
        : this(file, rec)
    {
        Text = text;
    }
       
    public InterfaceTexture(string file, Rect rec, Script script) //: this(file, rec, new RecDelPair(rec, script))
    {
        super(file, rec, new RecDelPair(rec, script));
    }
       
    public virtual void Draw()
    {
        if (Visible)
        {
            if (!Active)
            {
                Color backup = GUI.color;
                GUI.color = new Color(backup.r*.5f, backup.g*.5f, backup.b*.5f, backup.a*.5f);
                GUI.DrawTexture(DrawRect, _tex);
                GUI.color = backup;
            }
            else
            {
                GUI.DrawTexture(DrawRect, _tex);
            }
            if (Selected)
            {
                GUI.DrawTexture(selectRect, SelectTexture);
            }
            if (Text != null)
            {
                GUI.skin.GetStyle("Label").alignment = TextAnchor.MiddleCenter;
                GUI.Label(DrawRect, Text);
                GUI.skin.GetStyle("Label").alignment = TextAnchor.UpperLeft;
            }
        }
    }
    //this is so sets can call it with an offset
    public virtual void Update(Vector2 offset = new Vector2())
    {
        if (Active && Visible)
        {
            foreach (var inter in _interactions)
            {
                if (inter.Update(offset) && Selected)
                {
                    selectRect = inter._rec;
                }
            }
        }
    }
    public void ClearInteractions()
    {
        _interactions.Clear();
    }
    //null recs recommended. Otherwise specify the rect you want the interaction to take place in
    public void AppendToInteractions(Script s, Rect? r = null)
    {
        Rect rec = r ?? DrawRect;
        _interactions.Add(new RecDelPair(rec, s));
    }
    protected class RecDelPair
    {
        public Rect _rec;
        Script _script;
        public RecDelPair(Rect rec, Script script)
        {
            _rec = rec;
            _script = script;
        }
        public bool Update(Vector2 offset = new Vector2())
        {
            if (Input.GetMouseButtonDown(0) && _rec.Contains(InputHandler.MousePosition + offset))
            {
                _script();
                return true;
            }
            return false;
        }
    }
}



