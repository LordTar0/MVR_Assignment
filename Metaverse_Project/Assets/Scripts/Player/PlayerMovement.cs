using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MovementBase
{
    private static PlayerMovement instance;
    public static PlayerMovement _Instance { get => instance; }

    PlayerInputController inputController;
    bool InputDisabled;

    Goal Goal;

    //Gets the goal object to point the indicator towards it based on your location.
    public void GetGoal(Goal goal)
    {
        Goal= goal;
    }

    //Enable Player Input
    public void EnableInput()
    {
        inputController.ShipMovement.Enable();
        InputDisabled = false;

        inputController.ShipMovement.Quit.performed += QuitInput;
    }

    //Disable Player Input
    public void DisableInput()
    {
        inputController.ShipMovement.Disable();
        InputDisabled = true;

        inputController.ShipMovement.Quit.performed -= QuitInput;
    }


    protected override void Awake()
    {
        instance = this;
        base.Awake();
        inputController = new();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Input();

        //Throttle UI Updates
        if (PlayerUI._Instance != null)
        { 
            PlayerUI._Instance.UpdateCompassDirection(180+
                MathFunctions.AngleFromVec2Points(
                MathFunctions.GetTopDownVec2(transform.position), 
                MathFunctions.GetTopDownVec2(Goal.transform.position)));

            PlayerUI._Instance.UpdateThrottleBar(RB.velocity.magnitude/(MaxThrottleSpeed*10));
        }
    }

    private void Input()
    {
        if (InputDisabled) return;

        //Ship Movement input.
        Vector2 input = inputController.ShipMovement.Movement.ReadValue<Vector2>();
        Movement(input, MaxThrottleSpeed);
    }

    //TEMP (Fix in full game)
    private void QuitInput(InputAction.CallbackContext context)
    {
        GameManager._Instance.QuitGame();
    }
}