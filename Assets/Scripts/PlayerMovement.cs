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
    private float verticalVelocity;
    public Vector3 movementDirection;
    private Vector2 moveInput;
    private bool isRunning;

  
    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

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
    }
    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetmousePosition() - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        transform.forward = lookingDirection;
    }
    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * moveSpeed);

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

        
    }
   
}


