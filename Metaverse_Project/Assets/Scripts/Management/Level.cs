using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Goal Goal;
    [SerializeField] private AI_OBJ[] NPC_Objs;

    public Goal GetGoalObj() { return Goal; } //Returns the goal script to those who need it

    //Resets objects ready for the level
    public void ResetObjects()
    {
        Goal.Initialise();

        List<ObjectData> objs = new();

        objs.AddRange(NPC_Objs);

        foreach (var obj in objs) { obj.ResetObj(); }
    }

    //Starts the objects, this can be reverting the kinematic state or gravity usecases
    public void StartObjects()
    {
        List<ObjectData> objs = new();

        objs.AddRange(NPC_Objs);

        foreach (var obj in NPC_Objs) { obj.StartObj(); }
    }
}

[System.Serializable]
public abstract class ObjectData
{
    public Vector3 Position;

    public virtual void ResetObj(){
    }

    public virtual void StartObj(){
    }
}

[System.Serializable]
public class AI_OBJ : ObjectData
{
    public AIMovement NPCObj;
    public AI_Path curve;

    bool Default_IsKinematic;
    bool Default_UseGravity;
    Rigidbody RB;

    public override void ResetObj()
    {
        NPCObj.transform.position = Position;
        NPCObj.InitialiseNPC(curve);
        RB = NPCObj.GetComponent<Rigidbody>();
        Default_IsKinematic = RB.isKinematic;
        Default_UseGravity = RB.useGravity;
        RB.isKinematic = true;
        RB.useGravity = false;
    }

    public override void StartObj()
    {
        RB.isKinematic = Default_IsKinematic;
        RB.useGravity = Default_UseGravity;
    }
}