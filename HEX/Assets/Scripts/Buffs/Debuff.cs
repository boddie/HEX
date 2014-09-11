using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
	public abstract class Debuff : PEffect
	{
        public Debuff(int duration, string iconpath, int tickTime = 0)
            : base(duration, iconpath, tickTime)
        {
        }
	}
    interface SPDebuff
    {
        double spreduction
        {
            get;
        }
    }
    interface SDDebuff
    {
        double sdreduction
        {
            get;
        }
    }
    interface MSDebuff
    {
        double msreduction
        {
            get;
        }
    }
    interface Stun
    {
    }
    interface Immobilize
    {
    }
    interface Silence
    {
    }

}
