using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//An alternative to putting all of your textures in the same texture
class InterfaceTextureSet : HashSet<InterfaceTexture>
{
    private Dictionary<String, InterfaceTexture> namer;
    protected bool _alwaysSelected;
    protected InterfaceTexture _selected;
    public InterfaceTexture Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            if (_selected != null)
            {
                _selected.Selected = false;
            }
            _selected = value;
            if (value != null)
            {
                value.Selected = true;
            }
        }
    }
    public Texture SelectTexture
    {
        get
        {
            if (this.Count > 0)
            {
                return this.FirstOrDefault().SelectTexture;
            }
            return null;
        }
        set
        {
            foreach (var tex in this)
            {
                tex.SelectTexture = value;
            }
        }
    }
    public new void Clear()
    {
        base.Clear();
        namer.Clear();
    }

    public InterfaceTextureSet(bool alwaysSelected = false)
    {
        namer = new Dictionary<string, InterfaceTexture>();
        _alwaysSelected = alwaysSelected;
    }

    public virtual void Add(string name, InterfaceTexture tex)
    {
        namer.Add(name, tex);
        base.Add(tex);
    }
    public InterfaceTexture GetByName(string name)
    {
        return namer[name];
    }

    public virtual void Update()
    {
        if (_alwaysSelected && _selected == null && this.Count > 0)
        {
            _selected = this.FirstOrDefault();
            _selected.Selected = true;
        }
        foreach (InterfaceTexture it in this)
        {
            it.Update();
        }
    }
    public virtual void Draw()
    {
        foreach (InterfaceTexture it in this)
        {
            it.Draw();
        }
    }
}
