using UnityEngine;

public abstract class MovementBase : WaterPhysics
{

    [SerializeField] Transform Visuals;
    [SerializeField] ParticleSystem EngineParticles;
    [SerializeField] protected float MaxThrottleSpeed = 8;
    [SerializeField] float throttleTiltMax = 10;
    [SerializeField] float turnSpeed = 5;

    public void Movement(Vector2 input, float Throttle)
    {
        //Rotation

        //Sets the turn angle to be the Y rotation of the ship.
        float TurnAngle = transform.rotation.eulerAngles.y;

        //Checks to see if theres an input to update turn angle with a new angle using an input
        if (input.magnitude != 0)
        {
            TurnAngle = MathFunctions.FullRotationConversion(MathFunctions.AngleFromInput(input.normalized));
            EngineParticles.Play();
        }
        else{ EngineParticles.Stop();}

        //Checks the difference between the current ship angle and the input angle.
        float AngleDifference = (MathFunctions.GetFloatDifference(transform.rotation.eulerAngles.y, TurnAngle) / 180);

        //Gets the final rotation and lerps the boat to the new angles.
        Quaternion TiltRotation = Quaternion.Euler(-throttleTiltMax * input.magnitude, 0, 0);
        Quaternion TurnRotation = Quaternion.Euler(0, TurnAngle, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, TurnRotation, turnSpeed * Time.deltaTime);

        //Movement

        //Uses Rigidbody forces to push the boat in its forward direction based on the throttle speed and input magnitude.
        RB.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z) * Throttle * input.magnitude, ForceMode.Impulse);
    }
}