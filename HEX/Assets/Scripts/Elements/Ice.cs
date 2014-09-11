using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    class Ice : IElement
    {
        public static Ice Get = new Ice();
        private Ice()
        {
        }
        public bool StrongAgainst(IElement el)
        {
            return el.GetType() == typeof(Fire) || el.GetType() == typeof(Wind);
        }
        public bool WeakAgainst(IElement el)
        {
            return el.GetType() == typeof(Water) || el.GetType() == typeof(Earth);
        }
    }
}
