using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class AirMaterial : CMaterial
    {
        public AirMaterial() 
        {
            _cost = 25;
            DiffuseColor = Color.white;
            Health = 100;
            Element = Wind.Get;
        }
    }
}
