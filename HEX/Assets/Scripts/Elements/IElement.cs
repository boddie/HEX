using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Elements
{
    public interface IElement
    {
        bool WeakAgainst(IElement element);
        bool StrongAgainst(IElement element);
    }
    public class DefaultElement : IElement
    {
        public static DefaultElement Get = new DefaultElement();
        private DefaultElement()
        {
        }
        public bool WeakAgainst(IElement element)
        {
            return false;
        }
        public bool StrongAgainst(IElement element)
        {
            return false;
        }
        public static int Serialize(IElement element)
        {
            int serialized = -1;
            if (element.GetType() == typeof(DefaultElement))
            {
                serialized = 1;
            }
            else if (element.GetType() == typeof(Fire))
            {
                serialized = 2;
            }
            else if (element.GetType() == typeof(Earth))
            {
                serialized = 3;
            }
            else if (element.GetType() == typeof(Electricity))
            {
                serialized = 4;
            }
            else if (element.GetType() == typeof(Ice))
            {
                serialized = 5;
            }
            else if (element.GetType() == typeof(Water))
            {
                serialized = 6;
            }
            else if (element.GetType() == typeof(Wind))
            {
                serialized = 7;
            }
            else if (element.GetType() == typeof(Metal))
            {
                serialized = 8;
            }
            else if (element.GetType() == typeof(Nature))
            {
                serialized = 9;
            }
            if (serialized == -1)
            {
                throw new Exception("you are going to die. it will be an accident");
            }
            return serialized;
        }
        public static IElement Deserialize(int type)
        {
            IElement toReturn = null;
            switch (type)
            {
                case 1:
                    toReturn = DefaultElement.Get;
                    break;
                case 2:
                    toReturn = Fire.Get;
                    break;
                case 3:
                    toReturn = Earth.Get;
                    break;
                case 4:
                    toReturn = Electricity.Get;
                    break;
                case 5:
                    toReturn = Ice.Get;
                    break;
                case 6:
                    toReturn = Water.Get;
                    break;
                case 7:
                    toReturn = Wind.Get;
                    break;
                case 8:
                    toReturn = Metal.Get;
                    break;
                case 9:
                    toReturn = Nature.Get;
                    break;
            }
            if (toReturn == null)
            {
                throw new Exception("BY ALL THAT IS HOLY I WILL END YOU");
            }
            return toReturn;
        }
    }
    
}
