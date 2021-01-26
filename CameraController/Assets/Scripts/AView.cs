using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    [Range(0, 100)]
    public float weight;
    public bool isActiveOnStart;

    private void Start()
    {
        if (isActiveOnStart)
        {
            SetActive(true);
        }
    }

    public abstract CameraConfiguration GetConfiguration();

    void SetActive(bool isActive)
    {
        if (isActive)
            CameraController.Instance.AddView(this);
        else
            CameraController.Instance.RemoveView(this);
    }

    private void OnDrawGizmos()
    {
        GetConfiguration().DrawGizmos(Color.green);
    }
}
