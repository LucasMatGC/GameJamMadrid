using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3DControllerV3 : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private bool isBreaking;

    public GameObject player;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public float maxSteeringAngle = 30f;
    public float maxBreakForce = 5000f;
    public float motorForce = 3000f;
    public float brakeForce = 0f;

    private GameController gameControl;

    void Start()
    {
        gameControl = GameController.instance;
    }

    private void Update()
    {
        if(gameControl.IsGame3DRunning())
        {
            CheckIfUpsideDown();
            CheckRespawn();
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = maxBreakForce;
            frontRightWheelCollider.brakeTorque = maxBreakForce;
            rearLeftWheelCollider.brakeTorque = maxBreakForce;
            rearRightWheelCollider.brakeTorque = maxBreakForce;
        }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal3D");
        verticalInput = Input.GetAxis("Vertical3D");
        isBreaking = Input.GetButton("Breaks");
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        brakeForce = isBreaking ? maxBreakForce : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    private void CheckIfUpsideDown()
    {

        if (Mathf.Abs(Vector3.Dot(player.transform.up, Vector3.down)) < 0.125f)
        {

            gameControl.ShowUpsideDownText(true);

        }

    }

    private void CheckRespawn()
    {

        if (Input.GetButtonDown("Respawn"))
        {

            RespawnPlayer();

        }

    }

    private void RespawnPlayer()
    {

        Debug.Log("________________________________RESPAWN________________________________");
        player.transform.rotation = new Quaternion(0f, player.transform.rotation.y, 0f, player.transform.rotation.w);
        gameControl.ShowUpsideDownText(false);

    }

}