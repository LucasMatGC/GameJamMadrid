using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    public AudioSource impactSoundHit1;
    public AudioSource impactSoundHit2;
    public AudioSource impactSoundBox;
    public Rigidbody player = null;
    public Character3DControllerV3 characterController;

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

        if (player != null)
        {
            if (player.velocity.magnitude <= 1)
            {
                player.velocity = new Vector3(0, 0, 0);
                player.angularVelocity = new Vector3(0, 0, 0);
                characterController.brakeForce = characterController.maxBreakForce;
                characterController.brakeForce = 0;

            }
        }

    }
}

