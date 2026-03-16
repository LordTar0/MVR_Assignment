using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Timer timer;
    private Transform Player;

    [SerializeField] private float DistanceCorrection;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Player == null)
        {
            Player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == Player)
        {
            Player = null;
        }
    }

    private void FixedUpdate()
    {
        if (Player == null) { return; }

        if (MathFunctions.GetVector3Distance(Player.position, transform.position) < DistanceCorrection)
        {
            Debug.Log($"IM WORKING!");
        }
    }
}