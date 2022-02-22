/**
 * use this to move lights and objects around player for testting purposes
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControls : MonoBehaviour
{

    public Transform Target;
    
    public float RotationSpeed = 1;

    public float CircleRadius = 1;

    public float ElevationOffset = 0;
    public bool FaceTarget = false;
    private Vector3 positionOffset;
    private float angle;

    private void LateUpdate()
    {
        positionOffset.Set(
            Mathf.Cos(angle) * CircleRadius,
            ElevationOffset,
            Mathf.Sin(angle) * CircleRadius
        );
        transform.position = Target.position + positionOffset;  
        angle += Time.deltaTime * RotationSpeed;
        if (FaceTarget)
            transform.LookAt(Target);
    }

}
