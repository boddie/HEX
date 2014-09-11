using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class SteelMaterial : CMaterial
    {
        public SteelMaterial() 
        {
            _cost = 50;
            DiffuseColor = Colour.Get(200, 200, 200);
            Health = 200;
            Element = Metal.Get;
        }
    }
}
