using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
    class Haste : Buff, MSBuff
    {
        public double msincrease
        {
            get
            {
                return .25;
            }
        }
        public Haste()
            : base(10, "")
        {
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.MovementSpeed *= (1 + msincrease);
            Debug.Log("Buff applied: " + manager.CurrentPlayer.MovementSpeed);
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.MovementSpeed /= (1 + msincrease);
            Debug.Log("Buff cleaned: " + manager.CurrentPlayer.MovementSpeed);
        }
        protected override void ResolveCritical(PEffect other)
        {
            Debug.Log("Resolving: New duration is " + Duration);
        }
    }
}
