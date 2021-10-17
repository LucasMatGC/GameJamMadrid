using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3DController : MonoBehaviour
{

    private float MotorForce, SteerForce, BreakForce;
    //private float WheelCollider FR_L_Wheel, FR_R_Wheel, RE_L_Wheel, RE_R_Wheel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float verticalInput = Input.GetAxis("Vertical3D") * MotorForce;
        float horizontalInput = Input.GetAxis("Horizontal3D") * SteerForce;

        //RE_R_Wheel.motorTorque = verticalInput;
        //RE_L_Wheel.motorTorque = verticalInput;
        //
        //FR_L_Wheel.steerAngle = horizontalInput;
        //FR_R_Wheel.steerAngle = horizontalInput;
        //
        //if (Input.GetKeyDown("Backwards"))
        //{
        //
        //    RE_R_Wheel.brakeTorque = BreakForce;
        //    RE_L_Wheel.brakeTorque = BreakForce;
        //
        //}
        //
        //if (Input.GetKeyUp("Backwards"))
        //{
        //
        //    RE_R_Wheel.brakeTorque = 0;
        //    RE_L_Wheel.brakeTorque = 0;
        //
        //}
        //
        //if (Input.GetAxis("Vertical3D") == 0)
        //{
        //
        //    RE_R_Wheel.brakeTorque = BreakForce;
        //    RE_L_Wheel.brakeTorque = BreakForce;
        //
        //}
        //else
        //{
        //    RE_R_Wheel.brakeTorque = 0;
        //    RE_L_Wheel.brakeTorque = 0;
        //}
    }   
}
