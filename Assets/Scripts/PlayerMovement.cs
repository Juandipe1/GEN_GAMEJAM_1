using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    private Rigidbody rb;
    private Vector3 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Entrada horizontal (X) y vertical (Z)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;
    }

    private void FixedUpdate()
    {
        // Movimiento en el plano XZ
        rb.linearVelocity = moveInput * moveSpeed;
    }


}