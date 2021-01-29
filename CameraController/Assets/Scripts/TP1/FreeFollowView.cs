using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class FreeFollowView : AView
{
    [Header("Bottom Configuration")]
    [Range(-90, 90)]
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
    private Vector3 curvePosition;
    [Range(0, 1)]
    public float t = 0.5f;
    [Min(0)]
    public float curveSpeed;


    private float actualPitch = 0;
    private float actualRoll = 0;
    private float actualFov = 60f;

    private void OnDrawGizmos()
    {
        curve.DrawGizmo(Color.blue, GetTransformMatrix());

        SetView();
        GetConfiguration().DrawGizmos(Color.green);
    }

    public Matrix4x4 GetTransformMatrix()
    {
        return Matrix4x4.TRS(target.transform.position, Quaternion.Euler(0, yaw, 0), Vector3.one);
    }

    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (IsActive)
        {
            /*horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");*/
            horizontal = Input.GetAxis("Mouse X");
            vertical = Input.GetAxis("Mouse Y");
        }
        
        yaw += horizontal * yawSpeed * Time.deltaTime;
        t -= vertical * curveSpeed * Time.deltaTime;

        SetView();
    }

    private void SetView()
    {
        t = Mathf.Clamp(t, 0f, 1f);
        curvePosition = curve.GetPosition(t, GetTransformMatrix());

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
        else if (t == 0)
        {
            actualPitch = pitchBottom;
            actualRoll = rollBottom;
            actualFov = fovBottom;
        }
        else if (t > 0.5f && t < 1)
        {
            actualPitch = (pitchMiddle * (1 - t) + pitchTop * (t - 0.5f)) / 0.5f;
            actualRoll = (rollMiddle * (1 - t) + rollTop * (t - 0.5f)) / 0.5f;
            actualFov = (fovMiddle * (1 - t) + fovTop * (t - 0.5f)) / 0.5f;
        }
        else if (t > 0 && t < 0.5f)
        {
            actualPitch = (pitchBottom * (0.5f - t) + pitchMiddle * t) / 0.5f;
            actualRoll = (rollBottom * (0.5f - t) + rollMiddle * t) / 0.5f;
            actualFov = (fovBottom * (0.5f - t) + fovMiddle * t) / 0.5f;
        }

        transform.position = curvePosition;
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
            distance = 0
        };
        return cameraConfiguration;
    }
}
