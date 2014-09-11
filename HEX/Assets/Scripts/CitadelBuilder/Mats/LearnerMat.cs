using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Elements;

namespace Assets.CitadelBuilder
{
    class LearnerMaterial : CMaterial
    {
        public LearnerMaterial() 
        {
            _cost = 50;
            //ideallsdy, this would change colors
            DiffuseColor = Colour.Get(0, 153, 0);
            Health = 200;
            Element = Wind.Get;
        }
        public override void PerformOnHitEffect(string attacker, IElement ofAttack, int damage)
        {
            base.PerformOnHitEffect(attacker, ofAttack, damage);
            Element = ofAttack;
        }
    }
}
