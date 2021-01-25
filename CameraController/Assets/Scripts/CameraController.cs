using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool useCameraConfiguration1 = true;
    public float lerpDuration = 1;
    public CameraConfiguration cameraConfiguration1;
    public CameraConfiguration cameraConfiguration2;

    private bool isMoving = false;

    void Update()
    {
        if (!isMoving)
        {
            if (useCameraConfiguration1)
                cameraConfiguration1.UpdateConfig(gameObject);
            else
                cameraConfiguration2.UpdateConfig(gameObject);
        }
        else
        {
            if (useCameraConfiguration1)
                isMoving = cameraConfiguration1.LerpConfig(gameObject, lerpDuration);
            else
                isMoving = cameraConfiguration2.LerpConfig(gameObject, lerpDuration);
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

    public bool LerpConfig(GameObject camera, float lerpDuration)
    {
        Camera test = camera.GetComponent<Camera>();

        lerpTest += Time.deltaTime;

        float newPitch = Mathf.Lerp(camera.transform.rotation.x, pitch, lerpTest);
        float newYaw = Mathf.Lerp(camera.transform.rotation.y, yaw, lerpTest);
        float newRoll = Mathf.Lerp(camera.transform.rotation.z, roll, lerpTest);
        //Vector3 newPivot = Vector3.Lerp(camera.transform.position - camera.transform.rotation * Vector3.back * distance, pivot, lerpTest);
        //float newDistance = Mathf.Lerp();
        float newFov = Mathf.Lerp(test.fieldOfView, fov, lerpTest);

        camera.transform.rotation = Quaternion.Euler(newPitch, newYaw, newRoll);
        //camera.transform.position = newPivot + camera.transform.rotation * Vector3.back * distance;
        test.fieldOfView = newFov;

        if (lerpTest >= lerpDuration)
            return false;
        return true;
    }
}
