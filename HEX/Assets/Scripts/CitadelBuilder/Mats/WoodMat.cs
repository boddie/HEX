using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class WoodMaterial : CMaterial
    {
        public WoodMaterial() 
        {
            _cost = 10;
            DiffuseColor = Colour.Get(121, 96, 76);
            Health = 30;
            Element = Nature.Get; 
        }
    }
}
