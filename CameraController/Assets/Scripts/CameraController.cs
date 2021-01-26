using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance = null;

    public static CameraController Instance
    {
        get
        {
            return instance;
        }
    }

    public bool useCameraConfiguration1 = true;
    public float lerpDuration = 5;
    public CameraConfiguration cameraConfiguration1;
    public CameraConfiguration cameraConfiguration2;
    public float transitionSpeed = 2;

    private List<AView> activeViews = new List<AView>();
    private CameraConfiguration currentConfig = new CameraConfiguration();
    private CameraConfiguration targetConfig = new CameraConfiguration();

    private bool isMoving = false;
    private bool lastConfiguration;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        lastConfiguration = useCameraConfiguration1;
    }

    void OnDrawGizmos()
    {
        if(useCameraConfiguration1)
            cameraConfiguration1.DrawGizmos(Color.green);
        else 
            cameraConfiguration2.DrawGizmos(Color.green);
    }

    void Update()
    {
        float totalYaw = 0;
        float totalPitch = 0;
        float totalRoll = 0;
        float totalFov = 0;
        float totalDistance = 0;
        Vector3 totalPivot = Vector3.zero;
        float totalWeight = 0;

        foreach (AView view in activeViews)
        {
            CameraConfiguration cameraConfiguration = view.GetConfiguration();

            totalYaw += view.weight * cameraConfiguration.yaw;
            totalPitch += view.weight * cameraConfiguration.pitch;
            totalRoll += view.weight * cameraConfiguration.roll;
            totalFov += view.weight * cameraConfiguration.fov;
            totalDistance += view.weight * cameraConfiguration.distance;
            totalPivot += view.weight * cameraConfiguration.pivot;
            totalWeight += view.weight;
        }

        CameraConfiguration newCameraConfiguration = new CameraConfiguration
        {
            yaw = totalYaw / totalWeight,
            pitch = totalPitch / totalWeight,
            roll = totalRoll / totalWeight,
            fov = totalFov / totalWeight,
            distance = totalDistance / totalWeight,
            pivot = totalPivot / totalWeight
        };

        targetConfig = newCameraConfiguration;

        // Vitesse amortie

        if (transitionSpeed * Time.deltaTime < 1)
        {
            currentConfig.yaw = currentConfig.yaw + (targetConfig.yaw - currentConfig.yaw) * transitionSpeed * Time.deltaTime;
            currentConfig.pitch = currentConfig.pitch + (targetConfig.pitch - currentConfig.pitch) * transitionSpeed * Time.deltaTime;
            currentConfig.roll = currentConfig.roll + (targetConfig.roll - currentConfig.roll) * transitionSpeed * Time.deltaTime;
            currentConfig.fov = currentConfig.fov + (targetConfig.fov - currentConfig.fov) * transitionSpeed * Time.deltaTime;
            currentConfig.distance = currentConfig.distance + (targetConfig.distance - currentConfig.distance) * transitionSpeed * Time.deltaTime;
            currentConfig.pivot = currentConfig.pivot + (targetConfig.pivot - currentConfig.pivot) * transitionSpeed * Time.deltaTime;
        }
        else
            currentConfig = targetConfig;

        currentConfig.UpdateConfig(gameObject);

        /*if (!isMoving)
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
        }*/
    }

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }
}

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
