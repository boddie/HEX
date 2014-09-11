using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    class Turret : CitadelBlock
    {
        PersistentData manager;
        public Turret()
            : base("Turret")
        {
            manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
        }
    }
}
