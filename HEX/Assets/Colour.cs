using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Colour
{
    /// <summary>
    /// out of 255
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Color Get(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
