using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Wind : IElement
    {
        public static Wind Get = new Wind();
        private Wind()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Ice) || el.GetType() == typeof(Earth);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Nature) || el.GetType() == typeof(Metal);
        }
    }
}
