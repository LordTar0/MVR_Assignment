using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Timer timer;
    private Transform Player;

    [SerializeField] private float DistanceCorrection;

    public Action Finish_Action;

    bool isFinished;

    public void Initialise()
    {
        isFinished = false;
        Player = null;
        timer = new Timer();
        ResetTimer();
        timer.DisableTimer();
    }

    private void ResetTimer()
    {
        timer.SetStartTime(3);
        timer.SetTimerToStartTime();
    }


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
        if (Player == null || isFinished) { return; }

        if (MathFunctions.GetVector3Distance(Player.position, transform.position) < DistanceCorrection)
        {
            if (!timer.CheckTimerIsRunning()){ timer.EnableTimer(); }
            else { PlayerUI._Instance.UpdateCountDownTimer(Mathf.RoundToInt(timer.CheckTimer())); }

            if (timer.IsTimerUp())
            {
                Finish();
            }
        }
        else if (timer.CheckTimerIsRunning())
        {
            PlayerUI._Instance.UpdateCountDownTimer(0);
            timer.DisableTimer();
            ResetTimer();
        }
    }

    public void Finish()
    {
        timer.DisableTimer();
        ResetTimer();
        isFinished = true;
        Finish_Action?.Invoke();
    }
}