using UnityEngine;
using System.Collections;

public class TumbleRotation : MonoBehaviour
{
    public float xAngular;
    public float yAngular;
    public float zAngular;

    void Awake()
    {
        xAngular *= Random.Range(0f, 1f);
        yAngular *= Random.Range(0f, 1f);
        zAngular *= Random.Range(0f, 1f);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.localRotation *= Quaternion.AngleAxis(xAngular * Time.deltaTime, Vector3.right) *
                                   Quaternion.AngleAxis(yAngular * Time.deltaTime, Vector3.up) *
                                   Quaternion.AngleAxis(zAngular * Time.deltaTime, Vector3.forward);
	}
}
