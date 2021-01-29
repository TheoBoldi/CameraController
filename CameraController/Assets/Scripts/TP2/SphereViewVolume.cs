using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;
    public float outerRadius;
    public float innerRadius;

    private float distance;

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

        ComputeSelfWeight();
    }

    public override float ComputeSelfWeight()
    {
        if (distance > outerRadius)
        {
            view.weight = 0;
        }

        else if(distance <= outerRadius && distance > innerRadius)
        {
            view.weight = (distance - innerRadius) / (outerRadius - innerRadius);
        }

        else if (distance < innerRadius)
        {
            view.weight = 1f;
        }

        Debug.Log(view.weight);

        return base.ComputeSelfWeight();
    }
}
