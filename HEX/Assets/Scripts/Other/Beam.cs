using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour 
{

    public Vector3 TargetPosition = Vector3.zero;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        this.transform.GetChild(0).position = TargetPosition;
        this.transform.GetChild(2).position = Vector3.Lerp(transform.position, TargetPosition, Mathf.PingPong(Time.time, 1));

        _lineRenderer.SetPosition(0, this.transform.localPosition);
        _lineRenderer.SetPosition(1, TargetPosition);
    }
}
