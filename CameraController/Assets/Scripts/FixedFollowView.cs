using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : AView
{
    [Range(-180, 180)]
    public float roll;
    [Range(0, 180)]
    public float yawOffsetMax = 45;
    [Range(0, 180)]
    public float pitchOffsetMax = 45;
    [Range(0, 180)]
    public float fov = 60;
    public GameObject target;
    public GameObject centralPoint;

    private float yaw;
    private float pitch;

    private void Update()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;

        //Vector3.Angle(target.transform.position - transform.position, centralPoint.transform.position - transform.position);

        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
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
            distance = 0
        };
        return cameraConfiguration;
    }
}
