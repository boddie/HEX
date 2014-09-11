using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class WatersCureSwipe : BaseSwipeBuffAbility
{
    protected override void UseCritical(SwipeAbilityUseContext abilityContext)
    {
        PersistentData p = GameObject.Find("Persistence").GetComponent<PersistentData>();
        if (p.CurrentPlayer.BuffManager.RemoveBuff(new Assets.Buffs.Renew(0.0)))
        {
            int healthToHeal = (int) Math.Round(Assets.Buffs.Renew.RenewBaseHeal * 5 * p.CurrentPlayer.SpellPower);
            p.networkHeal(p.CurrentPlayer.Alias, healthToHeal);
            p.giveEffect(p.CurrentPlayer.Alias, p.CurrentPlayer.Alias, new Assets.Buffs.Rejuvenate().Serialize(), abilityContext.CastingPlayer.SpellPower);
        }
    }
    public override float Cooldown
    {
        get { return 15.0f; }
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Water.Get; }
    }
}

