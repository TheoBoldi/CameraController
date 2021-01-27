using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop;
    private float length;
    [HideInInspector] public List<Vector3> pointList = new List<Vector3>();

    void Awake()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pointList.Add(gameObject.transform.GetChild(i).transform.position);
        }

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            length += Vector3.Magnitude(pointList[i] - pointList[i + 1]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            Gizmos.DrawLine(pointList[i], pointList[i+1]);
        }

        foreach (var point in pointList)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    public float GetLength()
    {
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        Vector3 point = new Vector3();

        if (!isLoop)
        {
            if (distance >= length)
            {
                return pointList[pointList.Count - 1];
            }

            if (distance <= 0)
            {
                return pointList[0];
            }
        }

        if (distance < 0)
        {
            distance += length;
        }

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            if (distance > Vector3.Magnitude(pointList[i] - pointList[i + 1]))
            {
                distance -= Vector3.Magnitude(pointList[i] - pointList[i + 1]);

                if (distance < 0)
                {
                    distance += length;
                }

                if (i + 1 >= pointList.Count - 1)
                {
                    i = -1;
                }
            }
            else
            {
                point = pointList[i] + Vector3.Normalize(pointList[i+1] - pointList[i]) * distance;
                break;
            }
        }

        return point;
    }
}
