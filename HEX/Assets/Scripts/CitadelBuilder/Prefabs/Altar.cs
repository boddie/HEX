using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class Altar : CitadelBlock
    {
        public static int AltarHealth { get; private set; }
        static Altar()
        {
            AltarHealth = 500;
        }
        public Altar() : base("Altar")
        {
            base.Mat = new AltarMaterial();
        }
    }
    class AltarMaterial : CMaterial
    {
        public AltarMaterial()
        {
            Health = Altar.AltarHealth;
            Element = DefaultElement.Get;
            DiffuseColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }
}
