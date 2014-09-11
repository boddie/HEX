using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class LeadLegs : Debuff, MSDebuff
    {
        public double msreduction { get; private set; }
        public LeadLegs()
            : base(4, "")
        {
            msreduction = 0.8;
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.MovementSpeed *= (1 - msreduction);
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.MovementSpeed /= (1 - msreduction);
            msreduction -= 0.2;
        }
        protected override void ResolveCritical(PEffect other)
        {
            msreduction = Math.Max(msreduction, (other as LeadLegs).msreduction);
        }
    }
}
