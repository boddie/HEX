using UnityEngine;
using System.Collections;

public class CameraZoon : MonoBehaviour 
{
    int maxZoon = 8;
    int zoom = 4;
    int counter;

	// Use this for initialization
	void Start () {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && counter != maxZoon)
        {
            Camera.main.fieldOfView -= zoom;
            counter += 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && counter != -maxZoon)
        {
            counter -= 1;
            Camera.main.fieldOfView += zoom;
        }
	}
}
