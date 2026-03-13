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
        for (int i = 0; i < Powerby; i++)
        {
            input *= input;
        }

        return input;
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

    public static float AngleFromVec2Points(Vector2 a, Vector2 b)
    {
        float adjacent = GetVector2Distance(a, new Vector2(b.x, a.y));
        float opposite = GetVector2Distance(new Vector2(b.x, a.y), b);

        return Mathf.Atan(opposite / adjacent) * Mathf.Rad2Deg;
    }

    #endregion

    #region Vector 2 Math

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