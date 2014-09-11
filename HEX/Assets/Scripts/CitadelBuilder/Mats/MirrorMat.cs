using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    class MirrorMaterial : CMaterial
    {
        PersistentData pd;
        public MirrorMaterial() 
        {
            _cost = 50;
            //ideally, this would be reflective
            DiffuseColor = Colour.Get(240, 240, 240);
            Health = 200;
            Element = Metal.Get;
            pd = GameObject.Find("Persistence").GetComponent<PersistentData>();
        }
        public override void PerformOnHitEffect(string attacker, IElement ofAttack, int damage)
        {
            base.PerformOnHitEffect(attacker, ofAttack, damage);
            pd.giveDamage("world", attacker, damage / 5);
            
        }
    }
}
