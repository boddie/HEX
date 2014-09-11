using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
    class IceBeamUse : Buff, Immobilize
    {
        public IceBeamUse()
            : base(5, "")
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
            if (Active)
            {
                manager.CurrentPlayer.Immobilized = false;
                manager.CurrentPlayer.Silenced = false;
                this.Active = false;
            }
        }
    }
}
