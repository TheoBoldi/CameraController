using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    private bool isTriggered;

    void Update()
    {
        if (isTriggered && !IsActive)
            SetActive(true);
        if (!isTriggered && IsActive)
            SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}
