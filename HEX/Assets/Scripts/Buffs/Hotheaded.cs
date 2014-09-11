using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Hotheaded : Buff, SPBuff
    {
        public double spincrease
        {
            get
            {
                return .25;
            }
        }
        public Hotheaded()
            : base(10, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.SpellPower *= (1 + spincrease);
        }        
        public override void CleanUp()
        {
            manager.CurrentPlayer.SpellPower /= (1 + spincrease);
        }
        protected override void ResolveCritical(PEffect other)
        {
        }
    }
}
