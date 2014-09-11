using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Soft : Debuff, SDDebuff
    {
        public double sdreduction { get; private set; }
        public Soft()
            : base(5, "")
        {
            sdreduction = .25;
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
            //just needs to resolve duration which is automatic
        }
    }
}
