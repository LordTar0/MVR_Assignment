using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Placement_Curve : BezierCurve
{
    [SerializeField] GameObject GO_Prefab;
    private GameObject[] Objects;

    [ContextMenu("Update Path Objects")]
    protected override void UpdatePathing()
    {
        base.UpdatePathing();
        PlaceObjects();
    }

    private void PlaceObjects()
    {
        List<GameObject> list = new();
        if (Objects != null) { list.AddRange(Objects); }
        Objects = new GameObject[0];

        Debug.Log($"{list.Count}, {Points.Length}");

        if (list.Count > Points.Length)
        {
            for (int i = list.Count-1; i > Points.Length; i--)
            {
                list[i].SetActive(false);
                ExtraFunctions.SmartDestroy(list[i]);
                list.RemoveAt(i);
            }
        }

        if (list.Count < Points.Length)
        {
            for (int i = list.Count; i < Points.Length; i++)
            {
                GameObject newObj = Instantiate(GO_Prefab);
                newObj.transform.parent = transform;
                list.Add(newObj);
            }
        }

        Objects = list.ToArray();

        UpdatePathObjectLocation();
    }

    private void UpdatePathObjectLocation()
    {
        List<GameObject> list = new();
        list.AddRange(Objects);

        if (list.Count < Points.Length) { Debug.LogWarning($"Resolution of points was changed. Please update the pathing object count."); }

        for (int i = 0; i < list.Count; i++)
        {
            Vector2 Vec2Pos = Points[i] + MathFunctions.GetTopDownVec2(transform.position);
            list[i].transform.position = new Vector3(Vec2Pos.x, 0, Vec2Pos.y);
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        UpdatePathObjectLocation();
    }
}