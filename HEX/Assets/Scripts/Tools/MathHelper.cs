using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathHelper
{
    public static Vector3 GetWorldFromMousePos(Camera camera, Vector3 mousePosition)
    {
        Ray mouse = camera.ScreenPointToRay(mousePosition);
        RaycastHit rch;

        Physics.Raycast(mouse, out rch, float.MaxValue, 1 << 14);

        return new Vector3(rch.point.x, rch.point.y, rch.point.z);
    }

    public static Vector2 xy(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    public static Vector2 xz(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector2 yz(this Vector3 vector3)
    {
        return new Vector2(vector3.y, vector3.z);
    }
}
