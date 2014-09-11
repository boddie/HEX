using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.CitadelBuilder
{
    class TrashcanSelector : ISelectable
    {
        public static TrashcanSelector Get { get; set; }
        static TrashcanSelector()
        {
            Get = new TrashcanSelector();
        }
    }
}
