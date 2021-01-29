using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    [Range(0, 100)]
    public float weight = 1;

    protected bool IsActive;

    public abstract CameraConfiguration GetConfiguration();

    public void SetActive(bool isActive)
    {
        if (isActive)
            CameraController.Instance.AddView(this);
        else
            CameraController.Instance.RemoveView(this);
        IsActive = isActive;
    }

    private void OnDrawGizmos()
    {
        GetConfiguration().DrawGizmos(Color.green);
    }
}
