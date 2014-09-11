using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.CitadelBuilder
{
    enum RotationState
    {
        Default,
        Flat,
        View
    }
     
    //internal to CitadelBuilder
    static class GridHandler
    {
        static GridHandler()
        {
            RotateState = RotationState.Default;
        }
        internal static RotationState RotateState { get; private set; }
        private static float SquareDims
        {
            get
            {
                return (float)Citadel.REC_DIM / (float)Citadel.NUM_SQUARES;
            }
        }
        internal static Point GetPlaceOnGrid(Vector3 pos)
        {
            int x = Mathf.RoundToInt((pos.x + SquareDims / 2) / SquareDims);
            int z = Mathf.RoundToInt((pos.z + SquareDims / 2) / SquareDims);
            return new Point(x, z);
        }
        internal static Vector3 GetPlacePosition(Point pos, float y)
        {
            //pos will be some float (-recDim/2, recDim/2), (-recDim/2, recDim/2)
            //pos will be transformed into something [0, numSquares], [0, numSquares]
            //pos.X / squareDims+1, pos.Y / squareDims+1       
            return new Vector3(pos.X * SquareDims - SquareDims / 2, y, pos.Y * SquareDims - SquareDims / 2);
        }
        static readonly Vector3 flat = new Vector3(0, 9, 0);
        static readonly Quaternion flatrot = Quaternion.AngleAxis(90.0f, Vector3.right);
        static readonly Vector3 defaultpos = new Vector3(0, 9, -3);
        static readonly Quaternion defaultrot = Quaternion.AngleAxis(75.0f, Vector3.right);
        static readonly Vector3 viewpos = new Vector3(0, 1, -12);
        static readonly Quaternion viewrot = Quaternion.identity;
        public static void RotateX(Camera camera)
        {
            switch (RotateState)
            {
                case RotationState.Default:
                    camera.transform.position = flat;
                    camera.transform.rotation = flatrot;
                    RotateState = RotationState.Flat;
                    break;
                case RotationState.Flat:
                    camera.transform.position = viewpos;
                    camera.transform.rotation = viewrot;
                    RotateState = RotationState.View;
                    break;
                case RotationState.View:
                    camera.transform.position = defaultpos;
                    camera.transform.rotation = defaultrot;
                    RotateState = RotationState.Default;
                    break;
            }
        }
    }
}
