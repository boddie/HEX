using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Buffs
{
    public abstract class Buff : PEffect
    {
        public Buff(int duration, string iconPath, int tickTime = 0)
            : base(duration, iconPath, tickTime)
        {
        }
    }
    interface SPBuff 
    {
        double spincrease
        {
            get;
        }
    }
    interface MSBuff
    {
        double msincrease
        {
            get;
        }
    }
    interface SDBuff
    {
        double sdincrease
        {
            get;
        }
    }

}