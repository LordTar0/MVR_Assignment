using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AIMovement : MovementBase
{
    private int C_Waypoin_ID = -1;
    private Vector2 C_Point = Vector2.zero;
    [SerializeField] BezierCurve AI_Path;
    [SerializeField] bool ReverseDirection = false;
    [SerializeField,Min(0)] float CorrectionDistance;

    protected override void UpdateReferences()
    {
        base.UpdateReferences();
        UpdateWaypoint();
    }

    public void InitialiseNPC(AI_Path Path)
    {
        AI_Path = Path;
        C_Point = Vector2.zero;
        C_Waypoin_ID = -1;
        UpdateReferences();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //Checks the distance on a 2D plane between the NPC Boat & its next target
        if (MathFunctions.GetVector2Distance(MathFunctions.GetTopDownVec2(transform.position), C_Point) < CorrectionDistance)
        {
            UpdateWaypoint();
        }

        Movement(MathFunctions.GetVector2Direction(MathFunctions.GetTopDownVec2(transform.position), C_Point), MaxThrottleSpeed);
    }

    //Updates the next waypoint for the NPC Boat
    private void UpdateWaypoint()
    {
        if (AI_Path != null)
        {
            //Updates the Current Point
            C_Point = AI_Path.GetNextWaypoint(C_Waypoin_ID, ReverseDirection, out C_Waypoin_ID);
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateWaypoint();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        DEBUG_DrawTargetLine();
    }

    //Draws a line from the NPC Boat to its next point to see where its going.
    private void DEBUG_DrawTargetLine()
    {
        Vector3 myPos = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 TargetPos = new Vector3(C_Point.x, 1, C_Point.y);

        Gizmos.DrawLine(myPos, TargetPos);
    }
}
