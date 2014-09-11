using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]  

public class ExplosionMat : MonoBehaviour {
	
	bool doUpdate = false;
	public Texture2D ramp;
	
	// Use this for initialization
	void Start () {
		string[] kw = renderer.sharedMaterial.shaderKeywords;
		renderer.sharedMaterial = new Material(renderer.sharedMaterial);
		renderer.sharedMaterial.shaderKeywords = kw;
		renderer.sharedMaterial.SetTexture("_RampTex", ramp);
		doUpdate = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (doUpdate) {
			renderer.sharedMaterial.SetVector("_SpherePos", transform.position);
			float minscale = Mathf.Min(transform.lossyScale.x, Mathf.Min(transform.lossyScale.y, transform.lossyScale.z));
			renderer.sharedMaterial.SetFloat("_Radius", minscale/2 - 2);
		}
	}
}
