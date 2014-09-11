using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Furious : Buff, SDDebuff, SPBuff
    {
        public double spincrease
        {
            get
            {
                return 1.0;
            }
        }
        public double sdreduction
        {
            get
            {
                return .25;
            }
        }
        public Furious()
            : base(10, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.SpellPower *= (1 + spincrease);
            manager.CurrentPlayer.SpellDefense *= (1 - sdreduction);
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.SpellPower /= (1 + spincrease);
            manager.CurrentPlayer.SpellDefense /= (1 - sdreduction);
        }
        protected override void ResolveCritical(PEffect other)
        {
        }
    }
}
