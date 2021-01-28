using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    public bool isAuto;

    [Range(-180,180)]
    public float roll;
    [Min(0)]
    public float distance;
    [Range(0,180)]
    public float fov;

    public GameObject target;

    public Rail rail;
    public float railPosition;
    [Min(0)]
    public float speed = 1;

    private float yaw;
    private float pitch;

    private void OnDrawGizmos()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        if (isAuto)
        {
            SetAutoViewPosition();
        }
        transform.position = rail.GetPosition(railPosition);

        GetConfiguration().DrawGizmos(Color.green);
    }

    private void Update()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        if (isAuto)
        {
            SetAutoViewPosition();
        }
        else
        {
            SetViewPosition();
        }

        transform.position = rail.GetPosition(railPosition);
    }

    private void SetAutoViewPosition()
    {
        float min = float.MaxValue;
        int nearestPointId = 0;

        for (int i = 0; i < rail.pointList.Count; i++)
        {
            float tmpDistance = Vector3.Distance(target.transform.position, rail.pointList[i].position);
            if (tmpDistance < min)
            {
                min = tmpDistance;
                nearestPointId = i;
            }
        }

        Vector3 nearestPoint = rail.pointList[nearestPointId].position; // B
        Vector3 targetPos = target.transform.position; // T
        Vector3 BT = targetPos - nearestPoint;

        float leftNormalizedDistance = 0;
        float rightNormalizedDistance = 0;
        float mag_targetPos_leftResult = 0;
        float mag_targetPos_rightResult = 0;
        Vector3 leftResult = new Vector3();
        Vector3 rightResult = new Vector3();

        if (nearestPointId > 0)
        {
            Vector3 leftPoint = rail.pointList[nearestPointId - 1].position; // A
            Vector3 BA = leftPoint - nearestPoint;

            float BAmagnitude = Vector3.SqrMagnitude(BA);
            float BTDotBA = Vector3.Dot(BT, BA);

            leftNormalizedDistance = BTDotBA / BAmagnitude;

            leftResult = nearestPoint + leftNormalizedDistance * BA;
            mag_targetPos_leftResult = Vector3.Magnitude(targetPos - leftResult);
        }

        if (nearestPointId < rail.pointList.Count - 1)
        {
            Vector3 rightPoint = rail.pointList[nearestPointId + 1].position; // C
            Vector3 BC = rightPoint - nearestPoint;

            float BCmagnitude = Vector3.SqrMagnitude(BC);
            float BTDotBC = Vector3.Dot(BT, BC);

            rightNormalizedDistance = BTDotBC / BCmagnitude;

            rightResult = nearestPoint + rightNormalizedDistance * BC;
            mag_targetPos_rightResult = Vector3.Magnitude(targetPos - rightResult);
        }

        if (leftNormalizedDistance <= 0 && rightNormalizedDistance <= 0)
            railPosition = rail.GetLength(nearestPointId);
        else if (leftNormalizedDistance <= 0)
            railPosition = rail.GetLength(nearestPointId) + Vector3.Magnitude(nearestPoint - rightResult);
        else if (rightNormalizedDistance <= 0)
            railPosition = rail.GetLength(nearestPointId) - Vector3.Magnitude(nearestPoint - leftResult);
        else
        {
            if (mag_targetPos_leftResult <= mag_targetPos_rightResult)
                railPosition = rail.GetLength(nearestPointId) - Vector3.Magnitude(nearestPoint - leftResult);
            else
                railPosition = rail.GetLength(nearestPointId) + Vector3.Magnitude(nearestPoint - rightResult);
        }

        //transform.position = rail.pointList[nearestPoint];
    }

    private void SetViewPosition()
    {
        float move = Input.GetAxis("Horizontal");
        railPosition += move * speed * Time.deltaTime;

        while (railPosition < 0 || railPosition > rail.GetLength())
        {
            if (railPosition < 0)
                railPosition += rail.GetLength();
            else if (railPosition > rail.GetLength())
                railPosition -= rail.GetLength();
        }
    }

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration cameraConfiguration = new CameraConfiguration
        {
            yaw = yaw,
            pitch = pitch,
            roll = roll,
            fov = fov,
            pivot = transform.position,
            distance = distance
        };
        return cameraConfiguration;
    }
}
