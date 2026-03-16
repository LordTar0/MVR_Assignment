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
        
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 newPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, newPos, MoveSpeed * Time.deltaTime);
    }
}