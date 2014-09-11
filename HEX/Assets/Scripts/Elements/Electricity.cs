using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Electricity : IElement
    {
        public static Electricity Get = new Electricity();
        private Electricity()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Water) || el.GetType() == typeof(Wind) || el.GetType() == typeof(Metal);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Fire) || el.GetType() == typeof(Earth) || el.GetType() == typeof(Nature);
        }
    }
}
