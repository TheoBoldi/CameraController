using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    public float weight;
    public bool isActiveOnStart;

    private void Start()
    {
        if (isActiveOnStart)
        {
            SetActive(true);
        }
    }

    public  virtual CameraConfiguration GetConfiguration()
    {
        return null;
    }

    void SetActive(bool isActive)
    {

    }
}

public class FixedView : AView
{
    public float yaw;
    public float pitch;
    public float roll;
    public float fov;

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
