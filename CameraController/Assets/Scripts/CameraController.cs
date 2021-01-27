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

    public float transitionSpeed = 2;

    private List<AView> activeViews = new List<AView>();
    private CameraConfiguration currentConfig = new CameraConfiguration();
    private CameraConfiguration targetConfig = new CameraConfiguration();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
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
