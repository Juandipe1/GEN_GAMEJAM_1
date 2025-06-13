using UnityEngine;

public class GameInput : MonoBehaviour
{
    InputSystem_Actions playerInputActions;
    bool isPaused = false;
    bool canUseSpecial = true;
    [SerializeField] float specialTimer;


    void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
        playerInputActions.UI.Enable();
    }

    void Start()
    {
        specialTimer = 0;
    }

    void Update()
    {
        if (!canUseSpecial)
        {
            specialTimer -= Time.deltaTime;
            if (specialTimer <= 0)
            {
                canUseSpecial = true;
            }
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool IsAttacking()
    {
        bool isAttacking = playerInputActions.Player.Attack.WasPressedThisFrame();
        return isAttacking;
    }

    public bool IsSpecial()
    {
        bool isButtomPressed = playerInputActions.Player.SpecialAttack.WasPressedThisFrame();

        if (isButtomPressed && canUseSpecial)
        {
            canUseSpecial = false;
            specialTimer = 10;
            return true;
        }
        return false;
    }

    public bool TogglePauseIfPressed()
    {
        if (playerInputActions.UI.Pause.WasPressedThisFrame())
        {
            isPaused = !isPaused;
            return true;
        }

        return false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
