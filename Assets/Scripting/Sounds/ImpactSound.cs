using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource impactSound;
    public AudioSource impactSoundHit1;
    public AudioSource impactSoundHit2;
    public AudioSource impactSoundBox;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 5)
        {
            switch (collision.collider.tag)
            {

                case "Building":

                    if (!impactSoundHit1.isPlaying)
                    {
                        impactSoundHit1.Play();
                    }

                    break;

                case "Box":

                    if (!impactSoundBox.isPlaying)
                    {
                        impactSoundBox.Play();
                    }

                    break;

                case "Moviliary":

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

