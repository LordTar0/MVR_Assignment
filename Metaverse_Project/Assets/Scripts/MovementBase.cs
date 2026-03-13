using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    Rigidbody RB;
    [SerializeField] Transform Visuals;
    [SerializeField] ParticleSystem EngineParticles;
    [SerializeField] float ThrottleSpeed = 8;
    [SerializeField] float tiltMax = 40;
    [SerializeField] float throttleTiltMax = 10;
    [SerializeField] float turnSpeed = 5;

    protected virtual void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    public void Movement(Vector2 input)
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
        float AngleDifference = (MathFunctions.GetFloatDifference(transform.rotation.eulerAngles.y, TurnAngle) / 100);

        //Tilts the boat based on the angle difference
        float RollTilt = Mathf.Clamp(AngleDifference * tiltMax, -tiltMax, tiltMax);

        //Gets the final rotation and lerps the boat to the new angles.
        Quaternion TiltRotation = Quaternion.Euler(-throttleTiltMax * input.magnitude, 0, RollTilt);
        Quaternion TurnRotation = Quaternion.Euler(0, TurnAngle, 0);

        Visuals.localRotation = Quaternion.Lerp(Visuals.localRotation, TiltRotation, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, TurnRotation, turnSpeed * Time.deltaTime);

        //Movement

        //Uses Rigidbody forces to push the boat in its forward direction based on the throttle speed and input magnitude.
        RB.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z) * ThrottleSpeed * input.magnitude, ForceMode.Impulse);
    }
}