using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
	class Renew : Buff
	{
        public static readonly double RenewBaseHeal = 5;       
        int healthToRestore;
        public Renew(double spellpower)
            : base(12, "UI_Elements/PlaceHolder", 1)
        {
            healthToRestore = (int) Math.Round(RenewBaseHeal * spellpower);          
        }
        public override void ApplyEffect()
        {
            Debug.Log("Applying the Renew Effect");
            manager.networkHeal(manager.CurrentPlayer.Alias, healthToRestore);        
        }
        public override void CleanUp()
        {
            //no cleanup here!
        }
        protected override void ResolveCritical(PEffect other)
        {
            healthToRestore = Math.Max(healthToRestore, (other as Renew).healthToRestore);
        }
	}
}
