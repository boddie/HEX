using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class ObsidianMaterial : CMaterial
    {
        public ObsidianMaterial() 
        {
            _cost = 100;
            DiffuseColor = Colour.Get(20, 20, 20);
            Health = 500;
            Element = Earth.Get;
        }
    }
}
