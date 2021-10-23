using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claxon : MonoBehaviour
{
    public AudioSource claxonSound;
   
    void Update () {
		if(Input.GetButtonDown("HornButton")){
            claxonSound.Play();
        }
    }
}
