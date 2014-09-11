using UnityEngine;
using System.Collections;

public class FirestormRibbons : MonoBehaviour
{
    [SerializeField]
    private float _yRotationRate;
    [SerializeField]
    private float _xRotationRate;
    [SerializeField]
    private float _zRotationRate;

    [SerializeField]
    private Vector3 _distanceMagnitude;
    [SerializeField]
    private float _distanceScale;

    [SerializeField]
    private float _timeScale;

    private float _yOffset;

    void Awake()
    {
        _yOffset = 25 * Random.Range(-Mathf.PI, Mathf.PI);
    }

	void Update ()
    {
        float xPos = _distanceMagnitude.x * Mathf.Sin(Time.time * _xRotationRate * _timeScale + _yOffset);
        float zPos = _distanceMagnitude.z * Mathf.Cos(Time.time * _zRotationRate * _timeScale + _yOffset);
        float yPos = _distanceMagnitude.y * Mathf.Sin(Time.time * _yRotationRate * _timeScale);


        Vector3 position = new Vector3(xPos, yPos, zPos);
        position *= _distanceScale;


        transform.localPosition = position;
        

	}
}
