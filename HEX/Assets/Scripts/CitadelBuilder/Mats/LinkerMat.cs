using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class LinkerMaterial : CMaterial
    {
        private PersistentData pd;
        public LinkerMaterial() 
        {
            _cost = 50;
            //PURPLE!
            DiffuseColor = Colour.Get(128, 0, 153);
            Health = 200;
            Element = Nature.Get;
            pd = GameObject.Find("Persistence").GetComponent<PersistentData>();
        }
        public override void PerformOnHitEffect(string attacker, IElement ofAttack, int damage)
        {
            if (pd.CurrentPlayer.CurrentHealth > 0)
            {
                damage /= 2;
            }
            base.PerformOnHitEffect(attacker, ofAttack, damage);
        }
    }
}
