using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Knockeddown : Debuff, Stun
    {
        public Knockeddown()
            : base(1, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.Immobilized = true;
            manager.CurrentPlayer.Silenced = true;
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.Immobilized = false;
            manager.CurrentPlayer.Silenced = false;
        }
        protected override void ResolveCritical(PEffect other)
        {
        }
    }
}
