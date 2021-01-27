using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    public bool isAuto;

    [Range(-180,180)]
    public float roll;
    [Min(0)]
    public float distance;
    [Range(0,180)]
    public float fov;

    public GameObject target;

    public Rail rail;
    public float railPosition;
    [Min(0)]
    public float speed = 1;

    private float yaw;
    private float pitch;

    private void Update()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        if (isAuto)
        {
            float min = float.MaxValue;
            int nearestPoint = 0;

            for (int i = 0; i < rail.pointList.Count; i++)
            {
                float tmpDistance = Vector3.Distance(target.transform.position, rail.pointList[i]);
                if (tmpDistance < min)
                {
                    min = tmpDistance;
                    nearestPoint = i;
                }
            }

            transform.position = rail.pointList[nearestPoint];
        }
        else
        {
            float move = Input.GetAxis("Horizontal");
            railPosition += move * speed * Time.deltaTime;
            transform.position = rail.GetPosition(railPosition);
        }

    }

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration cameraConfiguration = new CameraConfiguration
        {
            yaw = yaw,
            pitch = pitch,
            roll = roll,
            fov = fov,
            pivot = transform.position,
            distance = distance
        };
        return cameraConfiguration;
    }
}
