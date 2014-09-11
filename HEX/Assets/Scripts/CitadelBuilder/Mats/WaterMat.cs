using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class WaterMaterial : CMaterial
    {
        public WaterMaterial() 
        {
            _cost = 25;
            DiffuseColor = Colour.Get(51, 105, 255);
            Health = 100;
            Element = Water.Get;
        }
    }
}
