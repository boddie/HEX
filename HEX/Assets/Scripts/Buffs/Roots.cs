using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Roots : Debuff, Immobilize
    {
        public Roots()
            : base(1, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.Immobilized = true;
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.Immobilized = false;
        }
        protected override void ResolveCritical(PEffect other)
        {
        }
    }
}
