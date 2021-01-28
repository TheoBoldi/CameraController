using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        if (t > 1 || t < 0)
        {
            //a verifier
            return A + t * (B - A);
        }
        else
        {
            return (1 - t) * A + t * B;
        }
    }

    public static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        if (t > 1 || t < 0)
        {
            //a verifier
            return Mathf.Pow((1 - t), 2) * A + 2 * (1 - t) * t * B + Mathf.Pow(t, 2) * C;
        }
        else
        {
            return (1 - t) * LinearBezier(A, B, t) + t * LinearBezier(B, C, t);
        }
    }

    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        if (t > 1 || t < 0)
        {
            //a verifier
            return Mathf.Pow((1 - t), 3) * A + 3 * Mathf.Pow((1 - t), 2) * t * B + 3 * (1 - t) * Mathf.Pow(t, 2) * C +
                   Mathf.Pow(t, 3) * D;
        }
        else
        {
            return (1 - t) * QuadraticBezier(A, B, C, t) + t * QuadraticBezier(B, C, D, t);
        }
    }
}
