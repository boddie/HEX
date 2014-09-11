using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Nature : IElement
    {
        public static Nature Get = new Nature();
        private Nature()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Water) || el.GetType() == typeof(Electricity) || el.GetType() == typeof(Wind);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Fire) || el.GetType() == typeof(Earth) || el.GetType() == typeof(Metal);
        }
    }
}
