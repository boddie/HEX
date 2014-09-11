using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.CitadelBuilder
{
    class BlockMover : ISelectable
    {
        private Light spot;
        private Point bs; 
        public static BlockMover Get { get; private set; }
        static BlockMover()
        {
            Get = new BlockMover();
        }
        public static void ReInitialize(Light sp)
        {
            Get = new BlockMover();
            Get.spot = sp;
        }
        public Point BlockSelected
        {
            get
            {
                return bs;
            }
            set
            {
                bs = value;
                spot.transform.position = GridHandler.GetPlacePosition(bs, 3.0f);
                Debug.Log("Spotlight has been moved!");
            }
        }
        public GameObject Block { get; set; }
        public BlockMover()
        {
            Block = null;
            bs = default(Point);
        }
    }
}
