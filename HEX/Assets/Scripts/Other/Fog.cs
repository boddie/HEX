using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour 
{
    private const float DISABLE_DIST = 60;
    private PersistentData persistence;

	void Start () 
    {
        persistence = GameObject.Find("Persistence").GetComponent<PersistentData>();
	}

    private bool _enable = true;
    private bool _active = false;
	
	void Update () 
    {
        if (_enable && this.GetComponent<ParticleSystem>().enableEmission)
        {
            _enable = false;
            _active = true;
        }
        if(_active)
        {
            if (persistence.CurrentPlayer.Position != null && Vector3.Distance(this.transform.position, persistence.CurrentPlayer.Position) < DISABLE_DIST)
            {
                this.GetComponent<ParticleSystem>().enableEmission = false;
            }
            else if (!this.GetComponent<ParticleSystem>().enableEmission)
            {
                this.GetComponent<ParticleSystem>().enableEmission = true;
            }
        }

	}
}
