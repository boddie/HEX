using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Fire : IElement
    {
        public static Fire Get = new Fire();
        private Fire()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Ice) || el.GetType() == typeof(Electricity) || el.GetType() == typeof(Nature);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Water) || el.GetType() == typeof(Earth) || el.GetType() == typeof(Wind);
        }
    }
}
