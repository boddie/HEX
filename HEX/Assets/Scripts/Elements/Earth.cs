using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Earth : IElement
    {
        public static Earth Get = new Earth();
        private Earth()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Fire) || el.GetType() == typeof(Electricity) || el.GetType() == typeof(Nature);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Ice) || el.GetType() == typeof(Water) || el.GetType() == typeof(Wind);
        }
    }
}
