using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AIMovement : MovementBase
{
    private int C_Waypoin_ID = -1;
    private Vector2 C_Point = Vector2.zero;
    [SerializeField] AI_Path AI_Path;
    [SerializeField] bool ReverseDirection = false;
    [SerializeField,Min(0)] float CorrectionDistance;

    protected override void Awake()
    {
        base.Awake();
        UpdateWaypoint();
    }

    private void FixedUpdate()
    {
        Vector3 Vec3WP = new Vector3(C_Point.x, transform.position.y, C_Point.y);

        if (MathFunctions.GetVector3Distance(transform.position, Vec3WP) < CorrectionDistance)
        {
            UpdateWaypoint();
        }

        Movement(MathFunctions.GetVector2Direction(new Vector2(transform.position.x, transform.position.z), C_Point));
    }

    //Updates the next waypoint for the NPC Boat
    private void UpdateWaypoint()
    {
        C_Point = AI_Path.GetNextWaypoint(C_Waypoin_ID, ReverseDirection, out C_Waypoin_ID);
    }

    private void OnValidate()
    {
        //Updates the Current Point
        if (AI_Path != null)
        {
            C_Point = AI_Path.GetNextWaypoint(C_Waypoin_ID, ReverseDirection, out C_Waypoin_ID);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        DEBUG_DrawTargetLine();
    }

    //Draws a line from the NPC to its next point to see where its going.
    private void DEBUG_DrawTargetLine()
    {
        Vector3 myPos = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 TargetPos = new Vector3(C_Point.x, 1, C_Point.y);

        Gizmos.DrawLine(myPos, TargetPos);
    }
}
