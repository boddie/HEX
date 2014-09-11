using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ScrollingInterfaceTexture : InterfaceTexture
{
    Rect _window;
    Vector2 _scrollPos;
    public static int scrollOffset = 18;
    private void Construct(Rect window, params RecDelPair[] interactions)
    {
        _window = window;
        _window.width += scrollOffset;
        _scrollPos = Vector2.zero;
        Selected = false;
        if (interactions.Length > 0)
        {
            Selected = true;
            selectRect = interactions.FirstOrDefault()._rec;
        }
    }
    //protected ScrollingInterfaceTexture(string file, Vector2 pos, Rect window, params RecDelPair[] interactions)
    //    : base(file, pos, interactions)
    //{
    //    Construct(window, interactions); 
    //}
    //protected ScrollingInterfaceTexture(string file, Rect rec, Rect window, params RecDelPair[] interactions)
    //        : base(file, rec, interactions)
    //{
    //    Construct(window, interactions); 
    //}
    public ScrollingInterfaceTexture(string file, Rect rec, Rect window)
        : base(file, rec)
    {
        Construct(window);
    }
    public ScrollingInterfaceTexture(string file, string text, Rect rec, Rect window)
        : base(file, rec, text)
    {
        Construct(window);
    }
    //protected ScrollingInterfaceTexture(string file, string text, Rect rec, Rect window, params RecDelPair[] interactions)
    //    : base(file, rec, text, interactions)
    //{
    //    Construct(window, interactions);
    //}
        public ScrollingInterfaceTexture(string file, Rect rec, Rect window, Script script)
            : base(file, rec, script)
    {
        Construct(window);
    }
        public ScrollingInterfaceTexture(string file, Vector2 pos, Rect window, Script script)
            : base(file, pos, script)
    {
        Construct(window);
    }
    public override void Draw()
    {
        if (base.Visible)
        {
            _scrollPos = GUI.BeginScrollView(_window, _scrollPos, DrawRect, false, false);
            base.Draw();
            GUI.EndScrollView();
        }
    }
    public override void Update(Vector2 offset = new Vector2()) //this is just to make sure it overrides the base method
    {
        if (Active)
        {
            foreach (var inter in _interactions)
            {
                var adjRec = new Rect(_window.x + _scrollPos.x, _window.y + _scrollPos.y, _window.width, _window.height); ;
                if (!(adjRec.Contains(new Vector2(inter._rec.xMin, inter._rec.yMin)) || adjRec.Contains(new Vector2(inter._rec.xMax, inter._rec.yMin)) || adjRec.Contains(new Vector2(inter._rec.xMin, inter._rec.yMax)) || adjRec.Contains(new Vector2(inter._rec.xMax, inter._rec.yMax))))
                {
                    continue;
                }
                if (inter.Update(_scrollPos) && Selected)
                {
                    selectRect = inter._rec;
                }
            }
        }
    }
}

