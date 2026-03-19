using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshCollider))]
public class Barrier_Curve : Placement_Curve
{
    private Mesh myMesh;
    private MeshCollider MyMeshCollider;

    [Header("Barrier Settings")]
    [SerializeField] private float BarrierHeight = 20f;
    [SerializeField] private bool FlipDirection = false;

    protected override void UpdatePathing()
    {
        base.UpdatePathing();
        if (MyMeshCollider == null) MyMeshCollider = GetComponent<MeshCollider>();
        UpdateBarrierMesh();
    }

    //Updates the mesh according to the points. Currently it needs a manual reset unlike the placed objects and Bezier Curve.
    private void UpdateBarrierMesh()
    {
        if (myMesh == null) myMesh = new();

        MyMeshCollider.sharedMesh = null;
        myMesh.Clear();
        myMesh.name = $"{this.name} Barrier";

        List<int> Triangles = new();
        List<Vector3> Verticies = new();


        for (int a = 0; a < Points.Length; a++)
        {
            int[] outTriPoints = new int[4];
            Vector3[] outVerts = new Vector3[4];

            GenerateSegment(a, out outTriPoints, out outVerts);

            Triangles.AddRange(outTriPoints);
            Verticies.AddRange(outVerts);
        }

        myMesh.vertices = Verticies.ToArray();
        myMesh.triangles = Triangles.ToArray();

        //Reveres the triangles to reverse the normals so that the collision detection is correct.
        if (FlipDirection) myMesh.triangles = myMesh.triangles.Reverse().ToArray();

        myMesh.RecalculateNormals(); //Recalculates the normals of the mesh so that lighting and physicis data works with the mesh.

        MyMeshCollider.sharedMesh = myMesh;

        Debug.Log($"UpdatedCollider \n Verticies Count:{myMesh.vertexCount}, Triangle Count:{myMesh.triangles.Length}");
    }

    //This generates each segment of the triangle, counting the inputted 'a' as the quad ID number & multiplying by 4 for the 4 points it has.
    private void GenerateSegment(int a, out int[] TrianglePoints, out Vector3[] Verticies)
    {
        int b = MathFunctions.ArrayLoop(Points.Length, a, false);

        int c = a * 4;

        GetSegmentMesh(Points[a], Points[b], c, out TrianglePoints, out Verticies);
    }

    //Generates the points and triangles of the quad mesh segment
    private void GetSegmentMesh(Vector2 Start_P, Vector2 End_P, int ID, out int[] TrianglePoints, out Vector3[] Verticies)
    {
        Verticies = new Vector3[] 
        {
            new Vector3(Start_P.x, transform.position.y - BarrierHeight / 2, Start_P.y),
            new Vector3(Start_P.x, transform.position.y + BarrierHeight / 2, Start_P.y),
            new Vector3(End_P.x, transform.position.y - BarrierHeight / 2, End_P.y),
            new Vector3(End_P.x, transform.position.y + BarrierHeight / 2, End_P.y)
        };

        TrianglePoints = new int[]
        {
            0+ID, 1+ID, 2+ID,
            1+ID, 3+ID, 2+ID
        };
    }
}