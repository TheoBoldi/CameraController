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
            if (lastConfiguration)
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

    [HideInInspector]
    public float timeElapsed;

    public void UpdateConfig(GameObject camera)
    {
        Camera cameraComponent = camera.GetComponent<Camera>();
        
        camera.transform.rotation = Quaternion.Euler(pitch, yaw, roll);
        camera.transform.position = pivot + camera.transform.rotation * Vector3.back * distance;
        cameraComponent.fieldOfView = fov;
    }

    public bool LerpConfig(GameObject camera, CameraConfiguration actualConfiguration, float lerpDuration)
    {
        Camera cameraComponent = camera.GetComponent<Camera>();

        if (timeElapsed < lerpDuration)
        {
            float newPitch = Mathf.Lerp(actualConfiguration.pitch, pitch, timeElapsed / lerpDuration);
            float newYaw = Mathf.Lerp(actualConfiguration.yaw, yaw, timeElapsed / lerpDuration);
            float newRoll = Mathf.Lerp(actualConfiguration.roll, roll, timeElapsed / lerpDuration);
            Vector3 newPivot = Vector3.Lerp(actualConfiguration.pivot, pivot, timeElapsed / lerpDuration);
            float newDistance = Mathf.Lerp(actualConfiguration.distance, distance, timeElapsed / lerpDuration);
            float newFov = Mathf.Lerp(actualConfiguration.fov, fov, timeElapsed / lerpDuration);

            camera.transform.rotation = Quaternion.Euler(newPitch, newYaw, newRoll);
            camera.transform.position = newPivot + camera.transform.rotation * Vector3.back * newDistance;
            cameraComponent.fieldOfView = newFov;

            timeElapsed += Time.deltaTime;
            return true;
        }
        else
        {
            timeElapsed = 0;
            return false;
        }
    }
}
