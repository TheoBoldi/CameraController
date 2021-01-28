using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public Vector3 D;

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A, B, C, D, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        return localToWorldMatrix.MultiplyPoint(GetPosition(t));
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;

        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(A), 0.5f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(B), 0.5f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(C), 0.5f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(D), 0.5f);

        for (int i = 0; i < 50; i++)
        {
            Gizmos.DrawLine(GetPosition(i / 50f, localToWorldMatrix), GetPosition((i + 1) / 50f, localToWorldMatrix));
        }
    }
}
