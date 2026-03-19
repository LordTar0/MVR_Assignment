using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float MoveSpeed;
    [SerializeField] Vector3 offset;


    private void Awake()
    {
        //If the camera hasn't got a reference to a target, it will search for the player.
        if(target == null) target = PlayerMovement._Instance?.transform;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    //Moves the Camera to the target position and offsets it by 'offset'
    private void MoveCamera()
    {
        Vector3 newPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, newPos, MoveSpeed * Time.deltaTime);
    }
}