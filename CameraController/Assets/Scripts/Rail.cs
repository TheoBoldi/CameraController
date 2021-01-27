using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop;

    private float length;
    [HideInInspector]
    public List<Transform> pointList = new List<Transform>();

    void Awake()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pointList.Add(gameObject.transform.GetChild(i).transform);
        }
    }

    private void Update()
    {
        length = 0;
        for (int i = 0; i < pointList.Count - 1; i++)
        {
            length += Vector3.Magnitude(pointList[i].position - pointList[i + 1].position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            Gizmos.DrawLine(pointList[i].position, pointList[i+1].position);
        }

        foreach (var point in pointList)
        {
            Gizmos.DrawSphere(point.position, 0.1f);
        }
    }

    public float GetLength()
    {
        return length;
    }

    public float GetLength(int indexLimit)
    {
        float test = 0;

        indexLimit++;
        if (indexLimit > pointList.Count)
            indexLimit = pointList.Count;

        for (int i = 0; i < indexLimit - 1; i++)
        {
            test += Vector3.Magnitude(pointList[i].position - pointList[i + 1].position);
        }
        return test;
    }

    public Vector3 GetPosition(float distance)
    {
        Vector3 point = new Vector3();

        if (!isLoop)
        {
            if (distance >= length)
                return pointList[pointList.Count - 1].position;

            if (distance <= 0)
                return pointList[0].position;
        }

        while (distance < 0 || distance > length)
        {
            if (distance < 0)
                distance += length;
            else if (distance > length)
                distance -= length;
        }

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            if (distance > Vector3.Magnitude(pointList[i].position - pointList[i + 1].position))
            {
                distance -= Vector3.Magnitude(pointList[i].position - pointList[i + 1].position);

                if (i + 1 >= pointList.Count - 1)
                {
                    i = -1;
                }
            }
            else
            {
                point = pointList[i].position + Vector3.Normalize(pointList[i+1].position - pointList[i].position) * distance;
                break;
            }
        }

        return point;
    }
}
