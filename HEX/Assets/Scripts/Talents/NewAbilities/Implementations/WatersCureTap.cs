using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;
using Assets.Buffs;

class WatersCureTap : BaseTapBuffAbility
{
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        PersistentData p = GameObject.Find("Persistence").GetComponent<PersistentData>();
        p.giveEffect(p.CurrentPlayer.Alias, p.CurrentPlayer.Alias, new Assets.Buffs.Renew(0.0).Serialize(), abilityContext.CastingPlayer.SpellPower);        
    }
    public override IElement Element
    {
        get { return Water.Get; }
    }
}

