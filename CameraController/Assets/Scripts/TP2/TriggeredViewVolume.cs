using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
            SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
            SetActive(false);
    }
}
