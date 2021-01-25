using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool useCameraConfiguration1 = true;
    public float lerpDuration = 5;
    public CameraConfiguration cameraConfiguration1;
    public CameraConfiguration cameraConfiguration2;

    private bool isMoving = false;
    private bool lastConfiguration;

    private void Awake()
    {
        lastConfiguration = useCameraConfiguration1;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (useCameraConfiguration1 == lastConfiguration)
            {
                if (useCameraConfiguration1)
                    cameraConfiguration1.UpdateConfig(gameObject);
                else
                    cameraConfiguration2.UpdateConfig(gameObject);
            }
            else
            {
                lastConfiguration = useCameraConfiguration1;
                isMoving = true;
            }
        }
        else
        {
            if (useCameraConfiguration1)
                isMoving = cameraConfiguration1.LerpConfig(gameObject, cameraConfiguration2, lerpDuration);
            else
                isMoving = cameraConfiguration2.LerpConfig(gameObject, cameraConfiguration1, lerpDuration);
        }
    }
}

[System.Serializable]
public class CameraConfiguration
{
    [Range(0, 360)]
    public float yaw = 180;
    [Range(-90, 90)]
    public float pitch = 0;
    [Range(-180, 180)]
    public float roll = 0;
    public Vector3 pivot;
    [Min(0)]
    public float distance = 5;
    [Range(0, 180)]
    public float fov = 60;

    private float lerpTest;

    public void UpdateConfig(GameObject camera)
    {
        Camera test = camera.GetComponent<Camera>();
        
        camera.transform.rotation = Quaternion.Euler(pitch, yaw, roll);
        camera.transform.position = pivot + camera.transform.rotation * Vector3.back * distance;
        test.fieldOfView = fov;
    }

    public bool LerpConfig(GameObject camera, CameraConfiguration actualConfiguration, float lerpDuration)
    {
        Camera test = camera.GetComponent<Camera>();

        if (lerpTest < lerpDuration)
        {
            float newPitch = Mathf.Lerp(actualConfiguration.pitch, pitch, lerpTest / lerpDuration);
            float newYaw = Mathf.Lerp(actualConfiguration.yaw, yaw, lerpTest / lerpDuration);
            float newRoll = Mathf.Lerp(actualConfiguration.roll, roll, lerpTest / lerpDuration);
            Vector3 newPivot = Vector3.Lerp(actualConfiguration.pivot, pivot, lerpTest);
            float newDistance = Mathf.Lerp(actualConfiguration.distance, distance, lerpTest);
            float newFov = Mathf.Lerp(actualConfiguration.fov, fov, lerpTest / lerpDuration);

            camera.transform.rotation = Quaternion.Euler(newPitch, newYaw, newRoll);
            camera.transform.position = newPivot + camera.transform.rotation * Vector3.back * newDistance;
            test.fieldOfView = newFov;

            lerpTest += Time.deltaTime;
            return true;
        }
        else
        {
            lerpTest = 0;
            return false;
        }
    }
}
