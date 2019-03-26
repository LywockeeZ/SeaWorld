using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public float CamDistance;
    public float moveSpeed = 1f;
    Vector3 targetPos;

    private void Awake()
    {
        //Target = FlockManager.Instance.FlockPrefab;
    }
    void Start()
    {
        CamDistance = transform.position.z;
    }

    void Update()
    {
        targetPos = FlockManager.Instance.Flocks[0].position;
        transform.position = Vector3.Slerp(transform.position,
                                           new Vector3(targetPos.x, targetPos.y, CamDistance), 
                                           moveSpeed * Time.deltaTime);
    }
}
