using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SpeedOfWindTap : BaseTapBuffAbility
{
    public override Assets.Elements.IElement Element
    {
        get { return Assets.Elements.Wind.Get; }
    }
    protected override void UseCritical(TapAbilityUseContext abilityContext)
    {
        PersistentData p = GameObject.Find("Persistence").GetComponent<PersistentData>();
        p.giveEffect(p.CurrentPlayer.Alias, p.CurrentPlayer.Alias, new Assets.Buffs.Haste().Serialize(), abilityContext.CastingPlayer.SpellPower);   
    }
}

