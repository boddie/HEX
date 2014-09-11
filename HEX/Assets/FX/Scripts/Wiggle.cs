using UnityEngine;
using System.Collections;

public class Wiggle : MonoBehaviour
{
    public float WiggleRate = 1.0f;
    public float WiggleMagnitude = 1.0f;

	void Update ()
    {
        float newX = WiggleMagnitude * Mathf.Cos(Time.time * WiggleRate);
        float newZ = WiggleMagnitude * Mathf.Sin(Time.time * WiggleRate);

        transform.localPosition = new Vector3(newX, transform.localPosition.y, newZ);
	}
}
