using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    [Range(0, 100)]
    public int priority = 0;
    public AView view;
    public bool isCutOnSwitch;

    protected bool IsActive { get; private set; }

    public virtual float ComputeSelfWeight()
    {
        return 1f;
        //return (priority * view.weight);
    }

    protected void SetActive(bool isActive)
    {
        if (isActive)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
            if (isCutOnSwitch)
                CameraController.Instance.isCutRequested = true;
        }
        else
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }
        IsActive = isActive;
    }
}
