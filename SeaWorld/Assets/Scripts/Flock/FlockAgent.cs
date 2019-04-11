using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    protected Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    protected Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    public virtual void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    //需要改进的地方
    public void Move(Vector3 velocity)
    {
        Vector3 _velocity = (Vector3)velocity;//new Vector3(velocity.x, velocity.y, 0);
        //transform.forward = velocity;
        //消除为零时的警告信息
        if (velocity.magnitude != 0)
        {
            Quaternion rotate = Quaternion.LookRotation(_velocity, Vector3.up);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, 5 * Time.deltaTime);
        }
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
