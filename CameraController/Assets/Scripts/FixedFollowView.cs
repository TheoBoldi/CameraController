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
        Vector3 dirCentralPoint = (centralPoint.transform.position - transform.position).normalized;

        float newYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float newPitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        float yawCentralPoint = Mathf.Atan2(dirCentralPoint.x, dirCentralPoint.z) * Mathf.Rad2Deg;
        float pitchCentralPoint = -Mathf.Asin(dirCentralPoint.y) * Mathf.Rad2Deg;

        float yawDiff = (newYaw - yawCentralPoint);
        float pitchDiff = (newPitch - pitchCentralPoint);

        if (yawDiff > 180)
            yawDiff -= 360;
        if (yawDiff < -180)
            yawDiff += 360;

        if (pitchDiff > 180)
            pitchDiff -= 360;
        if (pitchDiff < -180)
            pitchDiff += 360;

        if (Mathf.Abs(yawDiff) < yawOffsetMax)
            yaw = newYaw;
        else if (yawDiff <= -yawOffsetMax)
            yaw = yawCentralPoint - yawOffsetMax;
        else if (yawDiff >= yawOffsetMax)
            yaw = yawCentralPoint + yawOffsetMax;

        if (Mathf.Abs(pitchDiff) < pitchOffsetMax)
            pitch = newPitch;
        else if (pitchDiff <= - pitchOffsetMax)
            pitch = pitchCentralPoint - pitchOffsetMax;
        else if (pitchDiff >= pitchOffsetMax)
            pitch = pitchCentralPoint + pitchOffsetMax;
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
