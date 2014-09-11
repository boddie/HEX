using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Steel : Buff, SPBuff
    {
        public double spincrease { get; private set; }
        public Steel()
            : base(5, "")
        {
            spincrease = .25;
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
