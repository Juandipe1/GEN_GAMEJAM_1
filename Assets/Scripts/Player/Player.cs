using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;

    bool isWalking;
    bool isAttacking;
    bool isSpecial;

    [Header("Parameters")]
    [SerializeField] GameInput gameInput;
    [SerializeField] CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isAttacking && !isSpecial)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moverDir = new Vector3(inputVector.x, 0, inputVector.y);

        Vector3 move = moverDir * moveSpeed * Time.deltaTime;
        characterController.Move(move);

        isWalking = moverDir != Vector3.zero;

        if (isWalking)
        {
            float rotateSpeed = 100f;
            transform.forward = Vector3.Slerp(transform.forward, moverDir, rotateSpeed * Time.deltaTime);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void IsAttacking(bool IsAttack)
    {
        isAttacking = IsAttack;
    }

    public void IsSpecialAttack(bool IsSpecialAttack)
    {
        isSpecial = IsSpecialAttack;
    }

}
