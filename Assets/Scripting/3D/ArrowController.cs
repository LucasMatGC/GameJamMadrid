using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    private GameObject player;
    private GameObject objective;
    private GameObject arrow;
    private int minVerticalAngle = -12;
    private int maxVerticalAngle = 12;
    private float verticalAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player3D");
        objective = GameObject.Find("Destination");
        arrow = GameObject.Find("Arrow");
        //arrow.transform.rotation = Quaternion.LookRotation(objective.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

         var targetPosLocal = player.transform.InverseTransformPoint(objective.transform.position);
         var targetAngle = Mathf.Atan2(targetPosLocal.x, targetPosLocal.z) * Mathf.Rad2Deg;
         
        if (Mathf.Abs(targetAngle) > 90)
            verticalAngle = Mathf.LerpAngle(verticalAngle, maxVerticalAngle, 0.1f);
         else
            verticalAngle = Mathf.LerpAngle(verticalAngle, minVerticalAngle, 0.1f);

        arrow.transform.eulerAngles = new Vector3(verticalAngle, targetAngle, 0);

    }

    

}
