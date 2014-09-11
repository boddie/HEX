using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;
using Assets.Elements;
using UnityEngine;


class GustTap : PBAoEAbility
{
    public override string PrefabName
    {
        get { return "GustTap"; }
    }
    public override IElement Element
    {
        get { return Wind.Get; }
    }
    public override IAbilityBehavior CreateAbilityBehavior()
    {
        return new PBAoEBehavior(0, 0.25, PersistentData.Instance.CurrentPlayer.SpellPower, CollisionEffects);  
    }
    public override List<PEffect> CollisionEffects
    {
        get { return new List<PEffect>() { new Knockeddown() }; }
    }
}

