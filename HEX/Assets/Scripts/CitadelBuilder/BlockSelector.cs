using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.CitadelBuilder
{
    class BlockSelector : ISelectable
    {
        public Point BlockSelected { get; set; }
        public GameObject Block { get; set; }
        public BlockSelector(Point blockSelected)
        {
            BlockSelected = blockSelected;
        }
    }
}
