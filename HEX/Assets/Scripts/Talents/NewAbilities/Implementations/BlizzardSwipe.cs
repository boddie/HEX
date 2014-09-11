using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;

class BlizzardSwipe: TargetAoEAbility
{
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new FirestormBehavior(Damage, Radius, Duration);
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Fire.Get; }
    }
    protected override float Radius
    {
        get { return 80f; }
    }

    protected override int Damage
    {
        get { return 15; }
    }

    protected override float Duration
    {
        get { return 15f; }
    }

    public override float Cooldown
    {
        get { return 15.0f; }
    }

    public override string PrefabName
    {
        get { return "FirestormAbility"; }
    }

    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.All; }
    }

    public override List<PEffect> CollisionEffects
    {
        get { return null; }
    }
}
