using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Placement_Curve : BezierCurve
{
    [Header("Placement Settings")]
    [SerializeField] GameObject GO_Prefab;
    private GameObject[] Objects;


    #region PATH UPDATING TOOLS
    [ContextMenu("Update Path")] //Updates the pathing objects to match the new points amount. Will also be called on Awake.
    protected override void UpdatePathing()
    {
        base.UpdatePathing();
        PlaceObjects();
    }

    [ContextMenu("Hard Reset")] //This resets the path object, destroying all child objects in the process. Only use if objects become unsynced.
    private void HardClean()
    {
        if (transform.childCount == 0) { Debug.Log($"No objects to hard clean from {this.name}!"); return; }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            ExtraFunctions.SmartDestroy(transform.GetChild(i).gameObject);
        }

        Debug.Log($"HardClean Complete!");

        UpdatePathing();
    }
    #endregion

    #region PLACEMENT & MOVEMENT OF CURVE OBJECTS
    //Places objects based on the points of the curve.
    private void PlaceObjects()
    {
        List<GameObject> list = new(); //Creates new temp list

        //Checks to see if the object list is there and valid for use.
        if (PlacedOBJValidCheck()) { list.AddRange(Objects); Debug.Log($"Number of Points: {Points.Length} \n Number of Objects already placed: {Objects.Length}, Objects needing to be added/removed: {Objects.Length - Points.Length}"); }
        else { Debug.Log($"Creating Object list. Adding {Points.Length} new objects to the array."); }

        //Resets object array.
        Objects = new GameObject[0];


        //Deletes any excess objects
        if (list.Count > Points.Length)
        {
            for (int i = list.Count-1; i > Points.Length; i--)
            {
                list[i].SetActive(false);
                ExtraFunctions.SmartDestroy(list[i]);
                list.RemoveAt(i);
            }
        }

        //Creates new objects if needed.
        if (list.Count < Points.Length)
        {
            for (int i = list.Count; i < Points.Length; i++)
            {
                GameObject newObj = Instantiate(GO_Prefab);
                newObj.transform.parent = transform;
                list.Add(newObj);
            }
        }

        //Adds objects to the objects array
        Objects = list.ToArray();

        //Updates the objects location to their point location.
        UpdatePathObjectLocation();
    }


    //Updates the objects location based on curve points
    private void UpdatePathObjectLocation()
    {
        if (Objects == null) { PlaceObjects(); return; }

        if (Objects.Length < Points.Length) { Debug.LogWarning($"Resolution of points was changed. Please update the pathing object count."); }

        for (int i = 0; i < Points.Length; i++)
        {
            if (i >= Objects.Length) { break; }

            Vector2 Vec2Pos = Points[i] + MathFunctions.GetTopDownVec2(transform.position);
            Objects[i].transform.position = new Vector3(Vec2Pos.x, 0, Vec2Pos.y);
        }
    }

    //Checks to see if the Object array is valid as well as its listed objects, if not, it returns false.
    private bool PlacedOBJValidCheck()
    {
        if (Objects == null) { return false; }

        foreach (GameObject obj in Objects)
        {
            if (obj == null) return false;
        }
        return true;
    }

#endregion

    protected override void OnValidate()
    {
        base.OnValidate();
        UpdatePathObjectLocation();
    }
}