using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class StoneMaterial : CMaterial
    {
        public StoneMaterial() 
        {
            _cost = 50;
            DiffuseColor = Colour.Get(112, 112, 112);
            Health = 200;
            Element = Earth.Get;
        }
    }
}
