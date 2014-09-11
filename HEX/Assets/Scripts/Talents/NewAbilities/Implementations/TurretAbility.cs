using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.CitadelBuilder;
using Assets.Elements;
using Assets.Buffs;

class TurretAbility : MissileAbility
{
    private int _damage;
    private IElement _element;
    private string _name;
    
    public TurretAbility(CMaterial type)
    {
        _element = type.Element;
        _damage = type.Health / 4;
        string[] typer = type.GetType().ToString().Split('.');
        _name = typer.Last() + " Turret Ability";
    }
    public override IElement Element
    {
        get { return _element; }
    }
    protected override int Damage
    {
        get { return _damage; }
    }

    protected override float Speed
    {
        get { return 50; }
    }

    protected override float Range
    {
        get { return 100; }
    }

    protected override float SpawnPointOffsetDistance
    {
        get { return 7; }
    }

    public string MaterialName
    {
        get { return _name; }
    }
    public override string PrefabName
    {
        get { return "Fireball"; }
    }

    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }

    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>(); }
    }

    protected override float SpawnPointVerticalOffset
    {
        get { return 3.5f; }
    }
}

