using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFixedCamera : MonoBehaviour
{
    [SerializeField] private GameObject lookTarget;
    [SerializeField] private GameObject cameraTargetPosition;
    [SerializeField] private float smoothFallowSpeed = 10f;
    [SerializeField] private float heightFromTarget = 0.2f;

    private float pitch, yaw;
    private Vector3 smoothRotation;

    void LateUpdate()
    {
        if (lookTarget != null && cameraTargetPosition != null)
        {
            Vector3 currentPosition = cameraTargetPosition.transform.position;
            this.transform.position = Vector3.Lerp(transform.position, currentPosition, smoothFallowSpeed);

            Vector3 lookTargetVector = (lookTarget.transform.position - this.transform.position).normalized;
            lookTargetVector += new Vector3(0, heightFromTarget, 0);
            Quaternion toRotation = Quaternion.LookRotation(lookTargetVector, lookTarget.transform.up);
            transform.rotation = toRotation;
        }
    }
}
