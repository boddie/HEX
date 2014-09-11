using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Buffs;
using UnityEngine;

class FiresFuryTap : BaseTapBuffAbility
{
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        PersistentData p = GameObject.Find("Persistence").GetComponent<PersistentData>();
        p.giveEffect(p.CurrentPlayer.Alias, p.CurrentPlayer.Alias, new Hotheaded().Serialize(), abilityContext.CastingPlayer.SpellPower);
    }
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Fire.Get; }
    }
}

