using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted;

    // Update is called once per frame
    void Update()
    {
        
        if (hasStarted)
        {

            transform.position -= new Vector3(0f, -(beatTempo * Time.deltaTime), 0f);

        }


    }
}
