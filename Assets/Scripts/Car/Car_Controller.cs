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
    //public bool carActive { get; private set; }

    private PlayerControls controls;
    public Rigidbody rb { get; private set; }
    private float moveInput;
    private float steerInput;

    public float speed;

    public float turnSensetivity;
    [Header("Car Settings")]
    [SerializeField] private Transform centerOfMass;

    [Header("Engine Settings")]
    public float currentSpeed;
    [Range(7, 12)]
    public float maxSpeed;
    [Range(.5f, 5)]
    public float accelerationSpeed;
    [Range(1500, 3000)]
    public float motorForce = 1500f;


    [Header("Brakes Settings")]
    [Range(4, 10)]
    public float brakeSensetivity = 5;
    [Range(4000, 6000)]
    public float brakePower = 5000;
    private bool isBraking;

    private Car_Wheel[] wheels;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        controls = ControlsManager.instance.controls;
        ControlsManager.instance.SwitchToCarControls();

        wheels = GetComponentsInChildren<Car_Wheel>();

        AssignInputEvents();

        //ui = UI.instance;

        //SetupDefaultValues();
        //ActivateCar(false);
    }
    private void Update()
    {
        speed = rb.linearVelocity.magnitude;
    }

    private void FixedUpdate()
    {
        ApplyAnimationToWheels();
        ApplyDrive();
        ApplySteering();
        ApllyBrakes();
        ApplySpeedLimit();
    }

    private void ApplyDrive()
    {
        currentSpeed = moveInput * accelerationSpeed * Time.deltaTime;

        float motorTorqueValue = motorForce * currentSpeed;

        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
            {
                wheel.cd.motorTorque = motorTorqueValue;
            }
        }
    }

    private void ApplySpeedLimit()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
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
        float newBrakeTorque = brakePower * brakeSensetivity * Time.deltaTime;
        float currentBrakeTorque = isBraking ? newBrakeTorque : 0;

        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
                wheel.cd.brakeTorque = currentBrakeTorque;
        }    
    }

    private void ApplyAnimationToWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rotation;
            Vector3 position;

            wheel.cd.GetWorldPose(out position, out rotation);

            if (wheel.model != null)
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
