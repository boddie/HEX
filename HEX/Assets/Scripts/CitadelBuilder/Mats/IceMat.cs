using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class IceMaterial : CMaterial
    {
        public IceMaterial() 
        {
            _cost = 25;
            DiffuseColor = Colour.Get(82, 219, 255);
            Health = 100;
            Element = Ice.Get;
        }
    }
}
