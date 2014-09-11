using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    public enum SpireType
    {
        Regenerator,
        Shielder,
        AltarProtector
    }
    class Spire : CitadelBlock
    {
        public Spire() : base("")
        {
        }
    }
}
