using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    class PrefabSelector : ISelectable
    {
        public GameObject PrefabSelected { get; set; }
        public PrefabSelector(GameObject prefab)
        {
            PrefabSelected = prefab;
        }
    }
}
