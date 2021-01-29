using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;
    public float outerRadius;
    public float innerRadius;

    private float distance;

    private void Start()
    {
        view.weight = 0;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, innerRadius);
        Gizmos.DrawWireSphere(this.transform.position, outerRadius);
    }

    private void Update()
    {
        distance = Vector3.Distance(this.transform.position, target.transform.position);

        if (distance <= outerRadius && !IsActive)
        {
            SetActive(true);
        }

        else if(distance > outerRadius && IsActive)
        {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        if (distance > outerRadius)
        {
            return 0f;
        }

        else if (distance <= outerRadius && distance > innerRadius)
        {
            return 1 - ((distance - innerRadius) / (outerRadius - innerRadius));
        }

        else
        {
            return 1f;
        }
    }
}
