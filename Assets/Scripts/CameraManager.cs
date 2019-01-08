using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager> {

    private Transform targetTransform;
    private Vector3 cameraOffset;
    private float smoothFactor = 0.3f;
    private Vector3 smoothedPosition;

    private void Start()
    {
        targetTransform = GameObject.FindWithTag("targetForCamera").transform;
        cameraOffset = transform.position - targetTransform.position;
    }

    private void LateUpdate()
    {
        //provides us the camera follow the target
        smoothedPosition = Vector3.Lerp(transform.position, targetTransform.position + cameraOffset, smoothFactor);
        transform.position = smoothedPosition;
    }
}
