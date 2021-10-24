using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource impactSoundHit1;
    public AudioSource impactSoundHit2;
    public AudioSource impactSoundBox;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 5)
        {
            switch (collision.collider.tag)
            {

                case "Scenary":

                    if (!impactSoundHit1.isPlaying)
                    {
                        impactSoundHit1.Play();
                    }

                    break;

                case "Cardboard":

                    if (!impactSoundBox.isPlaying)
                    {
                        impactSoundBox.Play();
                    }

                    break;

                case "Car":

                    if (!impactSoundHit2.isPlaying)
                    {
                        impactSoundHit2.Play();
                    }

                    break;

                default:

                    break;
            }
        }

    }
}

