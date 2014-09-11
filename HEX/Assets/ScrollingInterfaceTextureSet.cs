using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class ScrollingInterfaceTextureSet : InterfaceTextureSet
{
    Rect _viewWindow;
    Vector2 _scrollPos;
    Rect totalRect
    {
        get
        {
            return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
        }
    }
    float ymax;
    float xmax;
    float xmin;
    float ymin;
    public ScrollingInterfaceTextureSet(Rect viewWindow)
    {
        _viewWindow = viewWindow;
        _scrollPos = new Vector2();
        ymin = float.MaxValue;
        xmin = float.MaxValue;
        xmax = 0;
        ymax = 0;
        _viewWindow.width += ScrollingInterfaceTexture.scrollOffset;
    }
    public override void Add(string name, InterfaceTexture tex)
    {
        RecalculateRect(tex.DrawRect);
        base.Add(name, tex);
    }
    public new void Add(InterfaceTexture tex)
    {
        RecalculateRect(tex.DrawRect);
        base.Add(tex);
    }
    private void RecalculateRect(Rect toAdd)
    {
        if (toAdd.yMax > ymax)
        {
            ymax = toAdd.yMax;
        }
        if (toAdd.yMin < ymin)
        {
            ymin = toAdd.yMin;
        }
        if (toAdd.xMin < xmin)
        {
            xmin = toAdd.xMin;
        }
        if (toAdd.xMax > xmax)
        {
            xmax = toAdd.xMax;
        }
    }
    public override void Update()
    {
        if (_alwaysSelected && _selected == null && this.Count > 0)
        {
            _selected = this.FirstOrDefault();
            _selected.Selected = true;
        }
        foreach (InterfaceTexture it in this)
        {
            var adjRec = new Rect(_viewWindow.x + _scrollPos.x, _viewWindow.y + _scrollPos.y, _viewWindow.width, _viewWindow.height); 
            //this unfortunately complicates things. try to stick to using single script interface textures
            if (!(adjRec.Contains(new Vector2(it.DrawRect.xMin, it.DrawRect.yMin)) || adjRec.Contains(new Vector2(it.DrawRect.xMax, it.DrawRect.yMin)) || adjRec.Contains(new Vector2(it.DrawRect.xMin, it.DrawRect.yMax)) || adjRec.Contains(new Vector2(it.DrawRect.xMax, it.DrawRect.yMax))))
            {
                continue;
            }
            it.Update(_scrollPos);
        }
    }
    public override void Draw()
    {
        _scrollPos = GUI.BeginScrollView(_viewWindow, _scrollPos, totalRect);
        base.Draw();
        GUI.EndScrollView();
    }
   
}

