using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Water : IElement
    {
        public static Water Get = new Water();
        private Water()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Fire) || el.GetType() == typeof(Earth) || el.GetType() == typeof(Metal);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Ice) || el.GetType() == typeof(Electricity) || el.GetType() == typeof(Nature);
        }
    }
}
