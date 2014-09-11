using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Drowning : Debuff
    {
        const int DrownBaseDamage = 5;
        int damageToDeal;
        public Drowning(double spellpower)
            : base(12, "", 3)
        {
            damageToDeal = (int)Math.Round(DrownBaseDamage * spellpower);
        }
        public override void ApplyEffect()
        {
            manager.giveDamage("Drowning", manager.CurrentPlayer.Alias, damageToDeal);
        }
        public override void CleanUp()
        {
        }
        protected override void ResolveCritical(PEffect other)
        {
            damageToDeal = Math.Max((other as Drowning).damageToDeal, damageToDeal);
        }
    }
}
