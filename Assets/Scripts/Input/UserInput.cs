using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    public bool JumpPress { get; private set; }
    public bool JumpHold { get; private set; }
    public bool JumpRelease { get; private set; }
    public bool TogglePress { get; private set; }
    public bool DashPress { get; private set; }
    public bool AttackPress { get; private set; }
    public bool MenuPress { get; private set; }

    PlayerInput playerInput;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction toggleAction;
    InputAction dashAction;
    InputAction attackAction;
    InputAction menuAction;

    void Awake()
    {
        if (instance == null)
            instance = this;

        playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    void Update()
    {
        UpdateInputs();
    }

    void SetupInputActions()
    {
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        toggleAction = playerInput.actions["Toggle"];
        dashAction = playerInput.actions["Dash"];
        attackAction = playerInput.actions["Attack"];
        menuAction = playerInput.actions["Menu"];
    }

    void UpdateInputs()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        JumpPress = jumpAction.WasPressedThisFrame();
        JumpHold = jumpAction.IsPressed();
        JumpRelease = jumpAction.WasReleasedThisFrame();
        TogglePress = toggleAction.WasPressedThisFrame();
        DashPress = dashAction.WasPressedThisFrame();
        AttackPress = attackAction.WasPressedThisFrame();
        MenuPress = menuAction.WasPressedThisFrame();
    }
}
