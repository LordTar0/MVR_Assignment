using System.Collections.Generic;
using UnityEngine;

public static class MathFunctions
{

    #region Basic Maths

    //Finds the difference float wise for two values, can be negative and positive.
    public static float GetFloatDifference(float one, float two)
    {
        return one - two;
    }

    //Finds the difference float wise for two values, always returns a positive value.
    public static float GetAbsoluteFloatDifference(float one, float two)
    {
        if (one > two)
        {
            return one - two;
        }
        else
        {
            return two - one;
        }
    }

    //Checks to make sure that the input value can be multiplied. Divide by 0 breaks things, though Wheatley wouldn't understand.
    public static float MultiCheck(float CheckValue)
    {
        return CheckValue <= 0 ? CheckValue : CheckValue >= 0 ? CheckValue : 0.000001f;
    }

    //A function for calculating the power of 'input'.
    public static float Powerby(float input, int Powerby)
    {
        float output = input;

        for (int i = 1; i < Powerby; i++)
        {
            output *= input;
        }

        return output;
    }

    public static int ArrayLoop(int Length, int ID, bool IsReversed)
    {
        int newID = ID + 1;

        if (IsReversed)
        {
            if (newID < 0) newID = Length - 1;
        }
        else
        {
            if (newID == Length) newID = 0;
        }

        return newID;
    }

    #endregion

    #region Quaternion Math

    public static float AngleFromVec2(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public static float AngleFromInput(Vector2 vector)
    {
        return Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
    }

    public static float FullRotationConversion(float angle)
    {
        if (angle < 0)
        {
            return 360 + angle;
        }

        return angle;
    }

    public static float RightAngleFromVec2Points(Vector2 a, Vector2 b)
    {
        float adjacent = GetVector2Distance(a, new Vector2(b.x, a.y));
        float opposite = GetVector2Distance(new Vector2(b.x, a.y), b);

        return Mathf.Atan(opposite / adjacent) * Mathf.Rad2Deg;
    }

    public static float AngleFromVec2Points(Vector2 a, Vector2 b)
    {
        float adjacent = GetVector2Distance(b, new Vector2(a.x, b.y));
        float opposite = GetVector2Distance(new Vector2(a.x, b.y), a);

        float angle = Mathf.Atan(adjacent / opposite) * Mathf.Rad2Deg;

        float newangle = a.y > b.y ? angle : 180 - angle;

        return a.x > b.x ? 360 - newangle : newangle;
    }

    #endregion

    #region Vector 2 Math

    public static Vector2 GetTopDownVec2(Vector3 Obj)
    {
        return new Vector2(Obj.x, Obj.z);
    }

    public static float GetVector2Distance(Vector2 Obj, Vector2 Target)
    {
        float x = GetAbsoluteFloatDifference(Obj.x, Target.x);
        float y = GetAbsoluteFloatDifference(Obj.y, Target.y);

        return Mathf.Sqrt(Powerby(x, 2) + Powerby(y, 2));
    }

    public static Vector2 GetVector2Direction(Vector2 Obj, Vector2 Target)
    {
        float x = Target.x - Obj.x;
        float y = Target.y - Obj.y;

        return new Vector2(x, y).normalized;
    }
    public static float Vec2Length(Vector2 v)
    {
        return Mathf.Sqrt(Powerby(v.x, 2) + Powerby(v.y, 2));
    }

    //Checks to make sure that the input value can be multiplied in a Vector 2 using 'MultiCheck'
    public static Vector2 Vec2MultiCheck(Vector2 CheckVector)
    {
        CheckVector.x = MultiCheck(CheckVector.x);
        CheckVector.y = MultiCheck(CheckVector.y);

        return CheckVector;
    }

    //Gets a list of points along a 'Quadratic' Bezier Curve, A = Start, B = Middle, C = End, Resolution = How many points along the line
    public static Vector2[] GetBezierCurve(Vector2 A, Vector2 B, Vector2 C, int Resolution)
    {
        List<Vector2> CurvePoints = new();

        //Clamps the Resolution so it's always between 3 and 100 for optimisation sake.
        Resolution = Mathf.Clamp(Resolution, 3, 100);

        for (int i = 0; i < Resolution; i++)
        {
            Vector2 point = new();
            float time = i / (float)Resolution;
            float unit = 1 - time;

            point += (Powerby(unit, 2) * A) + (2 * unit * time * B) + (Powerby(time, 2) * C);

            CurvePoints.Add(point);
        }

        return CurvePoints.ToArray();
    }

    #endregion

    #region Vector 3 Math

    //Finds the difference in a vector 3 space for two objects. Make sure the main object is 'Obj' and the target is 'target'.
    //Doing the reverse will result in an opposite value.

    public static float GetVector3Distance(Vector3 Obj, Vector3 Target)
    {
        float x = GetAbsoluteFloatDifference(Obj.x, Target.x);
        float y = GetAbsoluteFloatDifference(Obj.y, Target.y);
        float z = GetAbsoluteFloatDifference(Obj.z, Target.z);

        return Mathf.Sqrt(Powerby(x, 2) + Powerby(y, 2) + Powerby(z, 2));
    }

    public static float Vec3Length(Vector3 v)
    {
        return Mathf.Sqrt(Powerby(v.x, 2) + Powerby(v.y, 2) + Powerby(v.z, 2));
    }

    //Gets the direction of where target is compared to the obj
    public static Vector3 GetVector3Direction(Vector3 Obj, Vector3 Target)
    {
        float x = Target.x - Obj.x;
        float y = Target.y - Obj.y;
        float z = Target.z - Obj.z;

        return new Vector3(x, y, z).normalized;
    }
    #endregion

}


public static class ExtraFunctions
{
    public static void SmartDestroy(Object obj)
    {
        if (obj == null) return;

#if UNITY_EDITOR
        if (!Application.isPlaying) { GameObject.DestroyImmediate(obj); }
        else
#endif
        { GameObject.Destroy(obj); }
    }
}

public class Timer
{
    float C_Timer = -1;
    float TimerStart = 100;

    bool RunTimer = false;
    bool ReverseTimer = false;

    //Sets the current time to 'time'
    public void SetCurrentTime(float time) { C_Timer = time; }

    //Sets the start time to 'time'
    public void SetStartTime(float time) { TimerStart = time; }

    //Sets the current time to the start
    public void SetTimerToStartTime() { if (ReverseTimer) { C_Timer = 0; } else { C_Timer = TimerStart; } }

    //Enables & Disables the timer
    public void EnableTimer() { RunTimer = true; }
    public void DisableTimer() { RunTimer = false; }

    //Reverse timer will count up till the timer is greater than 'Start time', thus becomes 'End time'
    public void SetReverseTimer_Active() { ReverseTimer = true; }
    public void SetReverseTimer_Deactive() { ReverseTimer = false; }

    //Checks the current time
    public float CheckTimer() { return C_Timer; }

    //Checks to see if the timer is currently active
    public bool CheckTimerIsRunning() { return RunTimer; }

    //Spits out time in both seconds & miliseconds, mainly used for time displays.
    public void GetSecondsMiliseconds(out int Seconds, out int Miliseconds)
    {
        int timer_time = Mathf.RoundToInt(C_Timer * 100);
        Seconds = Mathf.RoundToInt(timer_time / 100);
        Miliseconds = Mathf.RoundToInt(Mathf.RoundToInt(timer_time) - Seconds * 100);
    }


    //Use this to update the time (i.e while(!IsTimerUp){return null} ).
    //The timer will stop afterwards unless specified (like Fixed update), so use wisely.
    public bool IsTimerUp()
    {
        if (!RunTimer) return false;

        if (ReverseTimer)
        {
            C_Timer += Time.deltaTime;
            if (C_Timer > TimerStart) { return true; }
        }
        else
        {
            C_Timer -= Time.deltaTime;
            if (C_Timer <= 0) return true;
        }

        return false;
    }
}