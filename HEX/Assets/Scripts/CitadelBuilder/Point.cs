using UnityEngine;
using System.Collections;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public Point(int x, int y)
        : this()
    {
        X = x;
        Y = y;
    }
    public override string ToString()
    {
        return "<" + X + "," + Y + ">";
    }
    public Point(string str)
        : this()
    {
        X = str[1];
        Y = str[3];
    }

    public static bool operator ==(Point one, Point two)
    {
        return one.X == two.X && one.Y == two.Y;
    }
    public static bool operator !=(Point one, Point two)
    {
        return !(one == two);
    }
    public override bool Equals(object obj)
    {
        return obj.GetType() == typeof(Point) && ((Point)obj) == this;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static Point Zero = new Point();
}