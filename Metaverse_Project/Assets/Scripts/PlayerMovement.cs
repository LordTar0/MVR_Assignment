using UnityEngine;

public class PlayerMovement : MovementBase
{
    PlayerInputController inputController;


    protected override void Awake()
    {
        base.Awake();

        inputController = new();

        inputController.ShipMovement.Enable();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 input = inputController.ShipMovement.Movement.ReadValue<Vector2>();

        Movement(input, MaxThrottleSpeed);
    }
}