using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Buffs
{
    class Rapid : Buff
    {
        public Rapid()
            : base(10, "")
        {
        }
        public override void ApplyEffect()
        {
            throw new NotImplementedException();
        }
        public override void CleanUp()
        {
            throw new NotImplementedException();
        }
        protected override void ResolveCritical(PEffect other)
        {
            throw new NotImplementedException();
        }
    }
}
