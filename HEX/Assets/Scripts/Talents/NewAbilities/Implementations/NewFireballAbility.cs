using System.Collections.Generic;
using Assets.Buffs;

public class FireballTap : MissileAbility
{
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return base.CreateAbilityBehavior();
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Fire.Get; }
    }

    protected override int Damage
    {
        get { return 25; }
    }

    protected override float Speed
    {
        get { return 50; }
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

    
}

