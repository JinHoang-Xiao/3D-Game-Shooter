using System;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement Data")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnspeed;
    private float speed;
    private float verticalVelocity;

    public Vector2 moveInput { get; private set; }
    private Vector3 movementDirection;

    private bool isRunning;

  
    private void Start()
    {
        player = GetComponent<Player>();

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = moveSpeed;

        AssignInputEvents();
    }
    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity);
        animator.SetFloat("zVelocity", zVelocity);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }
    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnspeed * Time.deltaTime);
    }
    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded == false)
        {
            verticalVelocity = verticalVelocity - 9.81f * Time.deltaTime;
            
        }
        else
        {
            verticalVelocity = -.5f * Time.deltaTime;
        }
        Vector3 gravityVector = new Vector3(0,verticalVelocity*Time.deltaTime, 0);
        characterController.Move(gravityVector);
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Move.canceled += context => moveInput = Vector2.zero;

        controls.Character.Run.performed += context =>
        {
                speed = runSpeed;
                isRunning = true;
        };
        controls.Character.Run.canceled += context =>
        {
            speed = moveSpeed;
            isRunning = false;
        };
    }
   
}


