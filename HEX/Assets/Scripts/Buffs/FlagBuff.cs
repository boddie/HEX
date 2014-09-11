using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Buffs
{
    class FlagBuff : Buff, MSBuff, SDBuff, SPBuff
    {
        private double _intensity;
        public double spincrease
        {
            get
            {
                return _intensity;
            }
        }
        public double sdincrease
        {
            get
            {
                return _intensity;
            }
        }
        public double msincrease
        {
            get
            {
                return _intensity;
            }
        }
        public FlagBuff(double intensity) : base(10, "UI_Elements/Placeholder")
        {
            _intensity = intensity;
        }
        public override void ApplyEffect()
        {
            manager.CurrentPlayer.SpellPower *= (1.0 + _intensity);
            manager.CurrentPlayer.SpellDefense *= (1.0 +  _intensity);
            manager.CurrentPlayer.MovementSpeed *= (1.0 + _intensity);
            Debug.Log("Movement Speed Buffed:" + manager.CurrentPlayer.MovementSpeed);
            //needs to do a network notification for particle effects purposes
        }
        public override void CleanUp()
        {
            manager.CurrentPlayer.SpellPower /= (1.0 + _intensity);
            manager.CurrentPlayer.SpellDefense /= (1.0 + _intensity);
            manager.CurrentPlayer.MovementSpeed /= (1.0 + _intensity);
            Debug.Log("Movement Speed Cleaned:" + manager.CurrentPlayer.MovementSpeed);
        }
        protected override void ResolveCritical(PEffect other)
        {
            //if (other.GetType() == typeof(FlagBuff))
            //{
            //    _intensity *= (1.0 + (other as FlagBuff)._intensity);
            //}
        }
    }
}
