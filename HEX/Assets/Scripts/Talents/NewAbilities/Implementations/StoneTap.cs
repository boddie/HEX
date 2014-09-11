using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using Assets.Buffs;


class StoneTap : MissileAbility
{
    protected override float Speed
    {
        get { return 40; }
    }
    protected override float SpawnPointVerticalOffset
    {
        get { return 3.5f; }
    }
    protected override float SpawnPointOffsetDistance
    {
        get { return 7; }
    }
    public override string PrefabName
    {
        get { return "StoneTap"; }
    }
    protected override float Range
    {
        get { return 50; }
    }
    protected override int Damage
    {
        get { return 0; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>() { new Petrification() }; }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
    public override IElement Element
    {
        get { return Earth.Get; }
    }
}

