using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Shocked : Debuff, Silence
    {
        public Shocked()
            : base(2, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.Silenced = true;
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.Silenced = false;
        }
        protected override void ResolveCritical(PEffect other)
        {
        }
    }
}
