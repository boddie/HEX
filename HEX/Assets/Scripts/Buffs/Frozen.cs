﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Frozen : Debuff, Stun
    {
        public Frozen()
            : base(1, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.Silenced = true;
            manager.CurrentPlayer.Immobilized = true;
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.Silenced = false;
            manager.CurrentPlayer.Immobilized = false;
        }
        protected override void ResolveCritical(PEffect other)
        {
        }
    }
}
