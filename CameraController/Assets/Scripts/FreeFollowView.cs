using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class FreeFollowView : AView
{
    [Header("Bottom Configuration")]
    [Range(-90,90)]
    public float pitchBottom;
    [Range(-180, 180)]
    public float rollBottom;
    [Range(0, 180)]
    public float fovBottom;

    [Header("Middle Configuration")]
    [Range(-90, 90)]
    public float pitchMiddle;
    [Range(-180, 180)]
    public float rollMiddle;
    [Range(0, 180)]
    public float fovMiddle;

    [Header("Top Configuration")]
    [Range(-90, 90)]
    public float pitchTop;
    [Range(-180, 180)]
    public float rollTop;
    [Range(0, 180)]
    public float fovTop;

    [Header("Camera Configuration")]
    [Range(-180, 180)]
    public float yaw;
    [Min(0)]
    public float yawSpeed;
    public GameObject target;
    public Curve curve;
    public Vector3 curvePosition;
    [Min(0)]
    public float curveSpeed;

    [Range(0,1)]
    private float t = 0.5f;

    private float actualPitch = 0;
    private float actualRoll = 0;
    private float actualFov = 60f;
    private float distance = 0;

    private void OnDrawGizmos()
    {
        curve.DrawGizmo(Color.blue, GetTransformMatrix());
    }

    public Matrix4x4 GetTransformMatrix()
    {
        return Matrix4x4.TRS(transform.position, Quaternion.Euler(0, yaw, 0), Vector3.one);
    }

    private void Update()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        //yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //actualPitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        transform.position = target.transform.position;

        float horizontal = Input.GetAxis("Horizontal");
        yaw += horizontal * yawSpeed * Time.deltaTime;

        float vertical = Input.GetAxis("Vertical");
        t += vertical * curveSpeed * Time.deltaTime;
        curvePosition = curve.GetPosition(t, GetTransformMatrix());

        if (t > 1)
        {
            t = 1;
        }
        else if(t < 0)
        {
            t = 0;
        }

        if (t == 1)
        {
            actualPitch = pitchTop;
            actualRoll = rollTop;
            actualFov = fovTop;
        }

        else if (t == 0.5f)
        {
            actualPitch = pitchMiddle;
            actualRoll = rollMiddle;
            actualFov = fovMiddle;
        }

        else if(t == 0)
        {
            actualPitch = pitchBottom;
            actualRoll = rollBottom;
            actualFov = fovBottom;
        }

        else if(t > 0.5f && t < 1)
        {
            actualPitch = (pitchMiddle * (t - 0.5f) + pitchTop * (1 - t)) / 0.5f;
            actualRoll = (rollMiddle * (t - 0.5f) + rollTop * (1 - t)) / 0.5f;
            actualFov = (fovMiddle * (t - 0.5f) + fovTop * (1 - t)) / 0.5f;
        }

        else if(t > 0 && t < 0.5f)
        {
            actualPitch = (pitchBottom * t + pitchMiddle * (0.5f - t)) / 0.5f;
            actualRoll = (rollBottom * t + rollMiddle * (0.5f - t)) / 0.5f;
            actualFov = (fovBottom * t + fovMiddle * (0.5f - t)) / 0.5f;
        }

        distance = Vector3.Distance(curvePosition, target.transform.position);
    }

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration cameraConfiguration = new CameraConfiguration
        {
            yaw = yaw,
            pitch = actualPitch,
            roll = actualRoll,
            fov = actualFov,
            pivot = transform.position,
            distance = distance
        };
        return cameraConfiguration;
    }
}
