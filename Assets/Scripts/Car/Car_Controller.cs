using System;
using UnityEngine;

//public enum DriveType { FrontWheelDrive, RearWheelDrive, AllWheelDrive }

//[RequireComponent(typeof(NavMeshObstacle))]
////[RequireComponent(typeof(Car_HealthController))]
////[RequireComponent(typeof(Car_Interaction))]
//[RequireComponent(typeof(BoxCollider))]
//[RequireComponent(typeof(Rigidbody))]
public class Car_Controller : MonoBehaviour
{
    //public Rigidbody rb { get; private set; }
    //public bool carActive { get; private set; }
    //public float speed;

    private PlayerControls controls;
    private float moveInput;
    private float steerInput;


    public float turnSensetivity;
    [Header("Engine Settings")]
    public float motorForce = 1500f;

    private bool isBraking;

    private Car_Wheel[] wheels;

    private void Start()
    {
        controls = ControlsManager.instance.controls;
        ControlsManager.instance.SwitchToCarControls();

        wheels = GetComponentsInChildren<Car_Wheel>();

        AssignInputEvents();

        //rb = GetComponent<Rigidbody>();
        //ui = UI.instance;

        //SetupDefaultValues();
        //ActivateCar(false);
    }

    private void FixedUpdate()
    {
        ApplyDrive();
        ApplySteering();
        ApllyBrakes();
    }

    private void ApplyDrive()
    {
        float motorTorqueValue = motorForce * moveInput * Time.deltaTime;

        foreach(var wheel in wheels)
        {
            if(wheel.axelType == AxelType.Front)
            {
                wheel.cd.motorTorque = motorTorqueValue;
            }
        }    
    }

    private void ApplySteering()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
            {
                float targetSteerAngle = steerInput * turnSensetivity;
                wheel.cd.steerAngle = Mathf.Lerp(wheel.cd.steerAngle, targetSteerAngle, .5f);
            }
        }
    }

    private void ApllyBrakes()
    {
        throw new NotImplementedException();
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rotation;
            Vector3 position;

            wheel.cd.GetWorldPose(out position, out rotation);

            if(wheel.model != null)
            {
                wheel.model.transform.position = position;
                wheel.model.transform.rotation = rotation;
            }


        }
    }

    private void AssignInputEvents()
    {
        controls.Car.Movement.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            moveInput = input.y;
            steerInput = input.x;
        };

        controls.Car.Movement.canceled += ctx =>
        {
            moveInput = 0;
            steerInput = 0;
        };

        controls.Car.Brake.performed += ctx => isBraking = true;
        controls.Car.Brake.canceled += ctx => isBraking = false;

    }
}
