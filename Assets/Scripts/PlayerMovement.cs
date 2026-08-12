using System;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    public CharacterController characterController;

    [Header("Movement Data")]
    [SerializeField] private float moveSpeed = 5f;
    public Vector3 movementDirection;

    [SerializeField] private float gravityScale = 9.81f;

    private float verticalVelocity;


    private Vector2 moveInput;
    private Vector2 aimInput;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Player.Move.canceled += context => moveInput = Vector2.zero;

        controls.Player.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Player.Aim.canceled += context => aimInput = Vector2.zero;
    }
    private void Start()
    {

    }
    private void Update()
    {
        ApplyMovement();
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
            verticalVelocity = verticalVelocity - gravityScale * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}

