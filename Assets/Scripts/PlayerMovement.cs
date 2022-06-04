    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] PlayerInputControl playerInputControl;
    [SerializeField] CharacterController playerCharacterController;

    [SerializeField] float gravityForce;
    [SerializeField] float playerSpeed;


    Vector3 yVelocity;

    [SerializeField] List<LayerMask> groundLayers;

    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;

    bool isGrounded;


    [SerializeField] float jumpHeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        playerInputControl = new PlayerInputControl();

        playerInputControl.PlayerOnFoot.Enable();

        playerInputControl.PlayerOnFoot.Jump.performed += Jump;
        playerInputControl.PlayerOnFoot.Run.performed += Run;
        playerInputControl.PlayerOnFoot.Run.canceled += Run;

    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Movement();
        ApplyGravity();
    }

    private void Movement()
    {

        float xMovement = playerInputControl.PlayerOnFoot.Movement.ReadValue<Vector2>().y;
        float yMovement = playerInputControl.PlayerOnFoot.Movement.ReadValue<Vector2>().x;

        Vector3 movementVector = transform.forward * xMovement + transform.right * yMovement;
        movementVector *= playerSpeed * Time.deltaTime;


        playerCharacterController.Move(movementVector);

    }

    private void ApplyGravity()
    {

        if (isGrounded && yVelocity.y < 0f)
            yVelocity.y = -2f;


        yVelocity.y += 5f * gravityForce * Time.deltaTime;

        playerCharacterController.Move(yVelocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        isGrounded = false;

        foreach(var layer in groundLayers)
        {

            isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, layer);

            if (isGrounded) break;

        }

    }

    private void Jump(InputAction.CallbackContext context)
    {

        if (isGrounded)
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);

        }

    }

    private void Run(InputAction.CallbackContext context)
    {

        if(context.phase == InputActionPhase.Performed)
        {

            playerSpeed *= 1.5f;

        }
        if(context.phase == InputActionPhase.Canceled)
        {

            playerSpeed /= 1.5f;

        }


    }


}
