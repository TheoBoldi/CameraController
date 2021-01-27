using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraConfiguration
{
    [Range(-180, 180)]
    public float yaw = 0;
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

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, roll);
    }

    public Vector3 GetPosition()
    {
        return pivot + GetRotation() * Vector3.back * distance;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
