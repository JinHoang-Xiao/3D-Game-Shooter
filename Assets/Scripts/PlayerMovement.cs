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

    public Vector3 movementDirection;
    private Vector2 moveInput;

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
        Vector3 lookingDirection = player.aim.GetmousePosition() - transform.position;
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
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f * Time.deltaTime;
        }
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Player.Move.canceled += context => moveInput = Vector2.zero;

        controls.Player.Run.performed += context =>
        {
                speed = runSpeed;
                isRunning = true;
        };
        controls.Player.Run.canceled += context =>
        {
            speed = moveSpeed;
            isRunning = false;
        };
    }
   
}


