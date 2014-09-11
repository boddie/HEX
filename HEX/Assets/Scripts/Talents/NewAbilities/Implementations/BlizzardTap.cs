using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;

class BlizzardTap : TargetAoEAbility
{
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new BlizzardBehavior(Damage, Radius, Duration);
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Ice.Get; }
    }
    protected override float Radius
    {
        get { return 25f; }
    }

    protected override int Damage
    {
        get { return 20; }
    }

    protected override float Duration
    {
        get { return 6f; }
    }

    public override float Cooldown
    {
        get { return 10.0f; }
    }

    public override string PrefabName
    {
        get { return "BlizzardAbility"; }
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
