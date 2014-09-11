using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour 
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

        _lineRenderer.SetPosition(0,this.transform.localPosition);

        for (int i = 1; i < 9; i++ )
        {
            Vector3 pos = Vector3.Lerp(this.transform.localPosition, TargetPosition, i / 9.0f);
            pos.x += Random.Range(-2f, 2f);
            pos.y += Random.Range(-2f, 2f);
            _lineRenderer.SetPosition(i, pos);
        }
    
        _lineRenderer.SetPosition(9, TargetPosition);
    }
}
