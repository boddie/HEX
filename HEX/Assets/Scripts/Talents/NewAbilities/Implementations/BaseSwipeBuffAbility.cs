using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;

abstract class BaseSwipeBuffAbility : BaseSwipeAbility
{
    public override string PrefabName
    {
        get { throw new NotImplementedException(); }
    }
    public override List<PEffect> CollisionEffects
    {
        get { throw new NotImplementedException(); }
    }
    public override AbilityTarget AbilityTarget
    {
        get { return AbilityTarget.Self; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        throw new NotImplementedException();
    }
}

