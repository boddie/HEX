using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using Assets.Buffs;

class NWTap : MissileAbility
{
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return base.CreateAbilityBehavior();
    }
    public override Assets.Elements.IElement Element
    {
        get { return Nature.Get; }
    }

    protected override int Damage
    {
        get { return 10; }
    }

    protected override float Speed
    {
        get { return 100; }
    }

    protected override float Range
    {
        get { return 200; }
    }

    protected override float SpawnPointOffsetDistance
    {
        get { return 7; }
    }
    public override string PrefabName
    {
        get { return "NWTap"; }
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

