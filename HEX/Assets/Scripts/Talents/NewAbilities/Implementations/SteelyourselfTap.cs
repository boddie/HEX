using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;
using Assets.Elements;

class SteelyourselfTap : MissileAbility
{
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Others; }
    }
    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>() { new LeadLegs() }; }
    }
    protected override int Damage
    {
        get { return 0; }
    }
    public override IElement Element
    {
        get { return Metal.Get; }
    }
    public override string PrefabName
    {
        get { return "SteelYourselfTap"; }
    }
    protected override float Range
    {
        get { return 100; }
    }
    protected override float SpawnPointVerticalOffset
    {
        get { return 3.5f; }
    }
    protected override float SpawnPointOffsetDistance
    {
        get { return 7; }
    }
    protected override float Speed
    {
        get { return 25; }
    }
}

