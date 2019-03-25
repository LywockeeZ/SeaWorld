using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    //该鱼在食物链中的等级
    public string Rank;

    public float idleSpeed = 1f;
    public float followSpeed = 1.5f;
    public float angleSpeed = 5f;
    public float maxDirectionChangeTime = 5f;
    [Range(1,50)]
    public float neighborRadius = 3f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    //AI的运动方向
    Vector3 direction = new Vector3(1,0,0);
    bool isRotating = false;
    bool isFollowing = false;
    //要跟随的目标
    GameObject _target = null;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    // Start is called before the first frame update
    void Start()
    {
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        StartCoroutine(ChangeDirection());
    }


    // Update is called once per frame
    void Update()
    {

        List<Transform> context = GetNearbyObjects();
        Vector2 move = CalculateMove(context);
        if (_target != null)
        {
            isFollowing = true;
            Follow(_target.transform.position, move);
        }
        else
        {
            isFollowing = false;
            Idle();
        }


    }


    void Idle()
    {
        Turn();
        Move(idleSpeed);
        
    }


    void Follow(Vector3 targetPos, Vector3 avoidanceMove)
    {
        direction = targetPos - transform.position;
        Turn();
        //Move(followSpeed);
        transform.position += followSpeed * new Vector3(direction.x, direction.y, 0).normalized * Time.deltaTime;
        transform.position += followSpeed * new Vector3(avoidanceMove.x, avoidanceMove.y, 0).normalized * Time.deltaTime;
    }


    void Turn()
    {
        Vector3 _direction = new Vector3(direction.x, direction.y, 0);
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


    void Move(float speed)
    {
        transform.position += speed * new Vector3(direction.x, direction.y, 0).normalized * Time.deltaTime;
    }


    public Vector2 CalculateMove(List<Transform> context)
    {
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //若需要分离，则执行分离操作
        Vector2 avoidanceMove = Vector2.zero;
        //需要分离的邻居的个数
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            if (Vector2.SqrMagnitude(item.position - transform.position) < SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(transform.position - item.position);
                //avoidanceMove = Vector2.SmoothDamp(agent.transform.forward, avoidanceMove, ref currentVelocity, agentSmoothTime);
                //avoidanceMove = Vector2.Lerp(agent.transform.forward, avoidanceMove, agentSmoothTime);
            }

        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        return avoidanceMove;
    }

    List<Transform> GetNearbyObjects()
    {
        List<Transform> context = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius);
        foreach (Collider c in colliders)
        {
            //获取同类鱼
            if (c.gameObject.tag == Rank)
            {
                context.Add(c.transform);
            }
            //获取要跟随的鱼
            if (c.gameObject.tag == "PlayerFlock")
            {
                _target = c.gameObject;
            }
        }
        return context;
    }


    //每间隔几秒换一个方向
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0, maxDirectionChangeTime + 1));
        //只有在闲逛状态下才随机改变方向
        if (!isFollowing)
        {
            direction = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
        }
        StartCoroutine(ChangeDirection());
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
    }

}
