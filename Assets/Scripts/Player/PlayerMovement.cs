using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] Vector2 moveDirection;
    Rigidbody playerRigidbody;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float jumpForce = 5;

    [Header("Ground Checker")]
    [SerializeField] bool isGrounded;

    void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame() && isGrounded) Jump();
    }

    void FixedUpdate()
    {
        Walking();
    }

    private void Jump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Walking()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + transform.right * moveDirection.x * walkSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

}
