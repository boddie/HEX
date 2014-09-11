using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class FireMaterial : CMaterial
    {
        public FireMaterial() 
        {
            _cost = 25;
            DiffuseColor = Colour.Get(255, 80, 0);
            Health = 100;
            Element = Fire.Get;
        }
    }
}
