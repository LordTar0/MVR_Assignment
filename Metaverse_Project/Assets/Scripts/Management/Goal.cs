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

    //Resets the completion cooldown
    private void ResetTimer()
    {
        timer.SetStartTime(3);
        timer.SetTimerToStartTime();
    }

    //Detects if the player has entered its bounding box

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Player == null)
        {
            Player = other.transform;
        }
    }
    
    //Detects if the player has left its bounding box

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == Player)
        {
            Player = null;
        }
    }

    private void FixedUpdate()
    {

        //Checks to see if the game has finished or if there is no player within its bounds
        if (Player == null || isFinished) { return; }



        if (MathFunctions.GetVector3Distance(Player.position, transform.position) < DistanceCorrection)
        {
            //Enables the timer when in the correct distance and updates the onscreen countdown to show how long you have left till completion
            if (!timer.CheckTimerIsRunning()){ timer.EnableTimer(); }
            else { PlayerUI._Instance.UpdateCountDownTimer(Mathf.RoundToInt(timer.CheckTimer())); }

            if (timer.IsTimerUp())
            {
                Finish();
            }
        }
        else if (timer.CheckTimerIsRunning()) 
        {
            //If the player is not within the distance required, it resets the clock to 3.
            PlayerUI._Instance.UpdateCountDownTimer(0);
            timer.DisableTimer();
            ResetTimer();
        }
    }

    public void Finish()
    {
        //Stops the clock, resets it and casts out an Action event to those who are subscribed.
        timer.DisableTimer();
        ResetTimer();
        isFinished = true;
        Finish_Action?.Invoke();
    }
}