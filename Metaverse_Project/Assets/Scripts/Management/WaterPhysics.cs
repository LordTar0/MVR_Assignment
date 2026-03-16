using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaterPhysics : MonoBehaviour
{
    protected Rigidbody RB;
    protected CapsuleCollider Collider;

    [SerializeField] LayerMask layerMask;
    [SerializeField] float RayHeightOffset;

    protected virtual void UpdateReferences()
    {
        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
    }

    protected virtual void FixedUpdate()
    {
        W_Physics();
    }

    protected virtual void Awake()
    {
        UpdateReferences();
    }

    private void W_Physics()
    {
        Vector3 buoyancy = -Physics.gravity * RB.mass/1.8f;

        if (Physics.Raycast(GetRay(0), RayHeightOffset*2, layerMask)) { RB.AddForceAtPosition(buoyancy, GetRay(0).origin);}
        if (Physics.Raycast(GetRay(1), RayHeightOffset*2, layerMask)) { RB.AddForceAtPosition(buoyancy, GetRay(1).origin);}
        if (Physics.Raycast(GetRay(2), RayHeightOffset*2, layerMask)) { RB.AddForceAtPosition(buoyancy, GetRay(2).origin);}
        if (Physics.Raycast(GetRay(3), RayHeightOffset*2, layerMask)) { RB.AddForceAtPosition(buoyancy, GetRay(3).origin);}
        if (Physics.Raycast(GetRay(4), RayHeightOffset*2, layerMask)) { RB.AddForceAtPosition(buoyancy, GetRay(4).origin);}
    }

    private Ray GetRay(int RayType) // 0 = Center (D), 1 = Front, 2 = Back, 3 = Right, 4 = Left
    {
        Ray ray = new();

        switch (RayType)
        {
            case 1: ray.origin = (transform.up * RayHeightOffset) + (transform.forward * Collider.height / 2) + Collider.center + transform.position; ; break; //Front
            case 2: ray.origin = (transform.up * RayHeightOffset) + (-transform.forward * Collider.height / 2) + Collider.center + transform.position; break; //Back
            case 3: ray.origin = (transform.up * RayHeightOffset) + (transform.right * Collider.radius) + Collider.center + transform.position; break; //Right
            case 4: ray.origin = (transform.up * RayHeightOffset) + (-transform.right * Collider.radius) + Collider.center + transform.position; break; //Left
            default: ray.origin = (transform.up * RayHeightOffset) + Collider.center + transform.position; ; break; //Default Center
        }

        ray.direction = Vector3.down;
        return ray;
    }

    protected virtual void OnValidate()
    {
        UpdateReferences();

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawRay(GetRay(0));
        Gizmos.DrawRay(GetRay(1));
        Gizmos.DrawRay(GetRay(2));
        Gizmos.DrawRay(GetRay(3));
        Gizmos.DrawRay(GetRay(4));
    }
}
