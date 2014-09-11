using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Electrified : Debuff, SDDebuff
    {
        public double sdreduction
        {
            get
            {
                return 0.9;
            }
        }
        public Electrified()
            : base(5, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.SpellDefense *= (1 - sdreduction);
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.SpellDefense /= (1 - sdreduction);
        }
        protected override void ResolveCritical(PEffect other)
        {
            
        }
    }
}
