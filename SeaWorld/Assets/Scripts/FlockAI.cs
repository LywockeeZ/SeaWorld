using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAI : MonoBehaviour
{
    public string Rank;
    public float idleSpeed = 1f;
    public float angleSpeed = 5f;
    public bool CanMove = true;
    public bool isInFlock = false;
    [Range(0, 50)]
    public float neighborRadius = 1f;
    public float maxDirectionChangeTime = 5f;


    Vector3 Direction = new Vector3(1, 0, 0);
    bool isRotating = false;
    float squareNeighborRadius;


    void Start()
    {
        squareNeighborRadius = neighborRadius * neighborRadius;
        StartCoroutine(ChangeDirection());
    }


    void Update()
    {
        if (!isInFlock)
        {
            Idle();
        }
        else
        {
            FlockMove();
            CheckAround();
        }
 
    }


    public void Idle()
    {
        //IdleTurn();
        Turn();
        Move(idleSpeed);
    }

    public void FlockMove()
    {
        Turn();
        Move(FlockManager.Instance.moveSpeed);
    }


    public void Move(float speed)
    {
        transform.position += speed * new Vector3(Direction.x, Direction.y, 0).normalized * Time.deltaTime;
    }


    public void Turn()
    {
        if (isInFlock)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isRotating = true;
                Direction = FlockManager.Instance.GetDirection();
                //Direction = FlockManager.Instance.GetMousePosition() - FlockManager.Instance.flockCenter;
                //Direction = FlockManager.Instance.GetMousePosition() - transform.position;
            }
        }
        else
        {
            isRotating = true;
        }
 
        Vector3 _direction = new Vector3(Direction.x, Direction.y, 0);
        

        if (isRotating)
        {
            //消除警告信息
            if (_direction.magnitude != 0)
            {
                Quaternion rotate = Quaternion.LookRotation(_direction, Vector3.up);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, angleSpeed * Time.deltaTime);
            }
            
            if (Vector3.Angle(_direction, transform.forward) < 0.01f)
            {
                isRotating = false;
            }
        }
    }


    public void IdleTurn()
    {
        Vector3 _direction = new Vector3(Direction.x, Direction.y, 0);
        isRotating = true;

        if (isRotating)
        {
            //计算出要旋转到目标方向的矩阵
            if (_direction.magnitude != 0)
            {
                Quaternion rotate = Quaternion.LookRotation(_direction, Vector3.up);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, angleSpeed * Time.deltaTime);
            }
            if (Vector3.Angle(_direction, transform.forward) < 0.01f)
            {
                isRotating = false;
            }
        }
    }


    public void CheckAround()
    {
        //List<Transform> context = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius, 1 << LayerMask.NameToLayer("Unflocked"));
        foreach (Collider c in colliders)
        {
            
            //如果在未聚集的曾，则改为已聚集的层，防止重复探测
            if (c.gameObject.layer == LayerMask.NameToLayer("Unflocked"))
            {
                c.gameObject.layer = 0;
                var _flockAI = c.gameObject.GetComponent<FlockAI>();               
                _flockAI.isInFlock = true;
                //使其与鱼群方向相同
                _flockAI.Direction = FlockManager.Instance.flockDirection;
                c.transform.rotation = FlockManager.Instance.flockRotation;
                FlockManager.Instance.Flocks.Add(c.transform);
            }
        }
    }



    //每间隔几秒换一个方向
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0, maxDirectionChangeTime + 1));
        //只有在闲逛状态下才随机改变方向
        if (!isInFlock)
        {
            Direction = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
        }
        StartCoroutine(ChangeDirection());
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
    }
}
