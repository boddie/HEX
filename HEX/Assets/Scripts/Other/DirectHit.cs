using UnityEngine;
using System.Collections;

public class DirectHit : MonoBehaviour 
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ability")
        {
            this.audio.Play();
        }
    }
}
