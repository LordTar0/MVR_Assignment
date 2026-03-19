using System.Collections.Generic;
using UnityEngine;

public abstract class BezierCurve : MonoBehaviour
{
    [Header ("Curve Settings")]
    [SerializeField] private Waypoint[] Waypoints;
    [SerializeField, Range(3,100)] private int Resolution = 10;
    protected Vector2[] Points;

    protected virtual void Start()
    {
        UpdatePathing();
    }

    #region CURVE CREATION

    //A public function to get the next waypoint location
    public Vector2 GetNextWaypoint(int C_ID, bool IsReversed, out int NewID)
    {
        int OutID = C_ID;

        if (Points == null) { 
            OutID = 0; 
            NewID = OutID; 
            return Vector2.zero; }

        if (IsReversed)
        {
            OutID--;

            if (OutID < 0) OutID = Points.Length - 1;
        }
        else
        {
            OutID++;

            if (OutID > Points?.Length - 1) OutID = 0;
        }

        NewID = OutID;
        return GetPointPosition(OutID);
    }

    //Updates the points list and where they should be located relative to the given vector2s
    private void UpdatePointList()
    {
        List<Vector2> points = new();

        for (int a = 0; a < Waypoints.Length; a++)
        {
            int b = MathFunctions.ArrayLoop(Waypoints.Length, a, false);

            Vector2 Start = Waypoints[a].GetWayPointLocal();

            Vector2 Middle = Waypoints[a].GetMiddlePointLocal();

            Vector2 End = Waypoints[b].GetWayPointLocal();

            points.AddRange(MathFunctions.GetBezierCurve(Start, Middle, End, Resolution));
        }

        Points = points.ToArray();
    }

    protected virtual void UpdatePathing()
    {
        UpdatePointList();
    }

    private Vector2 GetPointPosition(int Point)
    {
        if (Points?.Length > 0) return Points[Point] + MathFunctions.GetTopDownVec2(transform.position);

        return MathFunctions.GetTopDownVec2(transform.position);
    }


    private Vector2 GetEndPoint(int ID)
    {
        Vector2 Vec2End = GetPointPosition(ID);

        if (ID >= Points.Length - 1) { Vec2End = GetPointPosition(0); }
        else { Vec2End = GetPointPosition(ID + 1); }

        return Vec2End;
    }
    #endregion

    #region DEBUG INFO
    protected virtual void OnDrawGizmos()
    {
        DEBUG_DrawWaypoints();
        DEBUG_DrawPath();
    }

    protected virtual void DEBUG_DrawWaypoints()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {

            Vector3 Vec2Start = Waypoints[i].GetWayPointWorld(MathFunctions.GetTopDownVec2(transform.position));
            Vector3 Vec2Middle = Waypoints[i].GetMiddlePointWorld(MathFunctions.GetTopDownVec2(transform.position));

            Vector3 Start = new Vector3(Vec2Start.x, 1, Vec2Start.y);
            Vector3 Middle = new Vector3(Vec2Middle.x, 1, Vec2Middle.y);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Start, 0.2f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(Middle, 0.2f);
        }
    }

    protected virtual void DEBUG_DrawPath()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < Points.Length; i++)
        {
            Vector3 Start = new Vector3(GetPointPosition(i).x, 1, GetPointPosition(i).y);
            Vector3 End = new Vector3(GetEndPoint(i).x, 1, GetEndPoint(i).y);

            Gizmos.DrawLine(Start, End);

        }
    }
    #endregion

    protected virtual void OnValidate()
    {
        UpdatePointList();
    }
}

[System.Serializable]
public class Waypoint
{
    public Vector2 WayPoint;
    public Vector2 MiddlePoint;

    public Vector2 GetWayPointLocal()
    {
        return WayPoint;
    }

    public Vector2 GetWayPointWorld(Vector2 AddedTransform)
    {
        return WayPoint + AddedTransform;
    }

    public Vector2 GetMiddlePointLocal()
    {
        return MiddlePoint;
    }

    public Vector2 GetMiddlePointWorld(Vector2 AddedTransform)
    {
        return MiddlePoint + AddedTransform;
    }
}