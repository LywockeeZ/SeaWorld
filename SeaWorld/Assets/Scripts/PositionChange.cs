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
        Ray ray1 = new Ray(FlockManager.Instance.flockCenter, direction);
        Ray ray2 = new Ray(FlockManager.Instance.flockCenter + 8 * Vector3.down, direction);
        Debug.DrawRay(FlockManager.Instance.flockCenter + 8 * Vector3.down, direction);
        LayerMask mask = 1 << LayerMask.NameToLayer("Obstacle");
        RaycastHit hitInfo;
        if (FlockManager.Instance.Flocks.Count != 0)
        {
            targetPos = FlockManager.Instance.Flocks[0].position + direction.normalized * radius + 8 * Vector3.down;
        }
        else
        {
            targetPos = FlockManager.Instance.flockCenter + direction.normalized * radius + 8 * Vector3.down;
        }

        if (Physics.Raycast(ray1, out hitInfo, radius , mask) || Physics.Raycast(ray2, out hitInfo, radius + 3, mask))
        {
            if (Physics.Raycast(ray1, out hitInfo, radius, mask))
            {
                targetPos = FlockManager.Instance.flockCenter - direction.normalized * radius + 8 * Vector3.down;
            }
            else
            {
                targetPos = FlockManager.Instance.flockCenter - direction.normalized * radius + 8 * Vector3.up;
            }
        }

        return targetPos;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(FlockManager.Instance.flockCenter, radius);
    //}
}
