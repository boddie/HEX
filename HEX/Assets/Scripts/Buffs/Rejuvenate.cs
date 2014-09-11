using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Rejuvenate : Buff, SDBuff
    {
        public double sdincrease
        {
            get
            {
                return .25;
            }
        }
        public Rejuvenate()
            : base(10, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.SpellDefense *= (1 + sdincrease);
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.SpellDefense /= (1 + sdincrease);
        }
        protected override void ResolveCritical(PEffect other)
        {
            
        }
    }
}
