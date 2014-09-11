using UnityEngine;
using System.Collections;

public class StartPersistence : MonoBehaviour 
{
	void Awake () 
    {
        if (!GameObject.Find("Persistence"))
        {
            Object obj = Instantiate(Resources.Load("Persistence"));
            obj.name = "Persistence";
        }
	}
}
