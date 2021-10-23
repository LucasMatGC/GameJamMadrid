using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource impactSound;

    void OnCollisionEnter(Collision collision)
    {
      if(collision.collider.tag == "Player") {

            Debug.Log("colision "+ gameObject.name+" colision: "+ collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude > 5)
            {
                impactSound.Play();
            }
        }
    }

}
