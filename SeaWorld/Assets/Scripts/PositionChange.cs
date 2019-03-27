using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChange : MonoBehaviour
{
    public float radius = 1f;
    Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GetTargetPos();
    }


    public Vector3 GetTargetPos()
    {
        Vector3 direction = FlockManager.Instance.flockDirection;
        if (FlockManager.Instance.Flocks.Count != 0)
        {
            targetPos = FlockManager.Instance.Flocks[0].position + direction.normalized * radius;
        }
        else
        {
            targetPos = FlockManager.Instance.flockCenter + direction.normalized * radius;
        }
        
        return targetPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(FlockManager.Instance.flockCenter, radius);
    }
}
