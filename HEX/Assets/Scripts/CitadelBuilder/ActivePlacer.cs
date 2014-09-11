using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    class ActivePlacer : ISelectable
    {
        public static ActivePlacer Get { get; private set; }
        static ActivePlacer()
        {
            Get = new ActivePlacer(new Brick(), new WoodMaterial());
        }
        public static void ReInitialize()
        {
            Get = new ActivePlacer(new Brick(), new WoodMaterial());
        }
        public CitadelBlock PrefabSelected { get; set; }
        public CMaterial MaterialSelected { get; set; }
        public ActivePlacer(CitadelBlock prefab, CMaterial mat)
        {
            PrefabSelected = prefab;
            MaterialSelected = mat;
        }
    }
}
