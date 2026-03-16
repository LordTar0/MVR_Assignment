using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Curve : Placement_Curve
{
    private MeshCollider MeshCollider;

    protected override void UpdatePathing()
    {
        base.UpdatePathing();
        UpdateBarrierMesh();
    }

    private void UpdateBarrierMesh()
    {
        MeshCollider.sharedMesh.Clear();


    }
}