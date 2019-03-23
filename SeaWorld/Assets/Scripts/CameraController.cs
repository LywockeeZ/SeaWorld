using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    public float CamDistance;
    public float moveSpeed = 1f;
    Transform targetTrans;
    void Start()
    {
        targetTrans = Target.GetComponent<Transform>();
        CamDistance = transform.position.z;
    }

    void Update()
    {
        transform.position = Vector3.Slerp(transform.position,
            new Vector3(targetTrans.position.x, targetTrans.position.y, CamDistance), 
            moveSpeed * Time.deltaTime);
    }
}
