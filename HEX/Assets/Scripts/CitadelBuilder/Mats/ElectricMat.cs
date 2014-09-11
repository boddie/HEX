using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class ElecMaterial : CMaterial
    {
        public ElecMaterial() 
        {
            _cost = 25;
            DiffuseColor = Color.yellow;
            Health = 100;
            Element = Electricity.Get;
        }
    }
}
