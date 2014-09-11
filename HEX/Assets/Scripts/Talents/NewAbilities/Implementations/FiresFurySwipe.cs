using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;
using UnityEngine;

class FiresFurySwipe : BaseSwipeBuffAbility
{
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        PersistentData p = GameObject.Find("Persistence").GetComponent<PersistentData>();
        if (p.CurrentPlayer.BuffManager.RemoveBuff(new Assets.Buffs.Hotheaded()))
        {
            p.giveEffect(p.CurrentPlayer.Alias, p.CurrentPlayer.Alias, new Furious().Serialize(), abilityContext.CastingPlayer.SpellPower);
        }
    }
    public override Assets.Elements.IElement Element
    {
       get { return Assets.Elements.Fire.Get; }
    }
    public override float Cooldown
    {
        get { return 15.0f; }
    }
}

