using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Metal : IElement
    {
        public static Metal Get = new Metal();
        private Metal()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Wind) || el.GetType() == typeof(Nature);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Water) || el.GetType() == typeof(Electricity);
        }
    }
}
