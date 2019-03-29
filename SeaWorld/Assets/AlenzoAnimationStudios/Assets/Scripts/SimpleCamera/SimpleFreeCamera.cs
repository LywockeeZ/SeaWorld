using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFreeCamera : MonoBehaviour
{

    [SerializeField] private GameObject lookTarget;
    [SerializeField] private float mouseSencibility = 10f;
    [SerializeField] private float smoothFallowSpeed = 3f;
    [SerializeField] private float smoothYFallowSpeed = 8f;
    [SerializeField] private float distFromTarget = 10f;
    [SerializeField] private float highFromTarget = 2f;
    [SerializeField] private float distScrollSencibility = 0.5f;

    private float pitch, yaw;
    private Vector2 pitchYawClamp = new Vector2(-10, 80);
    private Vector3 smoothRotation;

    void LateUpdate()
    {
        if (lookTarget != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                distFromTarget -= distScrollSencibility;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                distFromTarget += distScrollSencibility;

            Vector3 currentPosition = new Vector3();

            pitch += Input.GetAxis("Mouse Y") * mouseSencibility;
            pitch = Mathf.Clamp(pitch, pitchYawClamp.x, pitchYawClamp.y);
            yaw += Input.GetAxis("Mouse X") * mouseSencibility;
            Vector3 deltaRotation = new Vector3(pitch, yaw);

            smoothRotation = Vector3.Lerp(smoothRotation, deltaRotation, Time.deltaTime * smoothFallowSpeed);
            this.transform.rotation = Quaternion.Euler(smoothRotation);

            currentPosition = lookTarget.transform.position + Vector3.up * highFromTarget - this.transform.forward * distFromTarget;
            this.transform.position = new Vector3(currentPosition.x, Mathf.Lerp(this.transform.position.y, currentPosition.y, Time.deltaTime * smoothYFallowSpeed), currentPosition.z);
        }
    }
}
