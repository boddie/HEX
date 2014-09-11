using UnityEngine;
using System.Collections;

public class KillPlaneScript : Photon.MonoBehaviour 
{
    PersistentData manager;

	void Start () 
    {
        manager = GameObject.Find("Persistence").GetComponent<PersistentData>();
	}

    void OnCollisionEnter(Collision c)
    {
        if (photonView.isMine && c.gameObject.tag == "killPlane")
        {
            manager.giveDamage(manager.CurrentPlayer.Alias, manager.CurrentPlayer.Alias, manager.CurrentPlayer.CurrentHealth);
        }
    }
}
