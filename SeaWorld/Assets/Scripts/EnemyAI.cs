using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //该鱼在食物链中的等级
    public string Rank;

    public float idleSpeed = 1f;
    public float followSpeed = 1.5f;
    public float angleSpeed = 5f;
    //闲逛时换方向的时间间隔
    public float maxDirectionChangeTime = 5f;
    //进入检测范围的半径
    [Range(1,50)]
    public float neighborRadius = 3f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    public ParticleSystem biteParticle;

    //AI的运动方向
    Vector3 direction = new Vector3(1,0,0);
    bool isRotating = false;
    bool isFollowing = false;
    bool isEscaping = false;
    //要跟随的目标
    GameObject _target = null;
    string myRankTag;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    // Start is called before the first frame update
    void Start()
    {
        myRankTag = gameObject.tag;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        StartCoroutine(ChangeDirection());
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        //获取周围的对象，同时给要跟随的对象赋值
        List<Transform> context = GetNearbyObjects();
        if (context != null)
        {
            //关闭了avoidance的计算
            move = Vector2.zero;//CalculateMove(context);
        }
        
        if (_target != null)
        {
            isFollowing = true;
            Follow(_target.transform.position, move);
        }
        else
        {
            isFollowing = false;
            if (isEscaping)
            {
                
                Idle(followSpeed);
            }
            else
            {
                Idle(idleSpeed);
            }      
        }


    }


    void Idle(float Speed)
    {
        Turn();
        Move(Speed);
        
    }

    //暂时不使用avoidanceMove
    public void Follow(Vector3 targetPos, Vector3 avoidanceMove)
    {
        direction = targetPos - transform.position;
        Turn();
        //Move(followSpeed);+ new Vector3(avoidanceMove.x, avoidanceMove.y, 0))
        transform.position += (followSpeed) * (new Vector3(direction.x, direction.y, direction.z) .normalized * Time.deltaTime);
    }


    void Turn()
    {
        Vector3 _direction = new Vector3(direction.x, direction.y, direction.z);
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
        transform.position += speed * new Vector3(direction.x, direction.y, direction.z).normalized * Time.deltaTime;
    }

    //计算维持距离需要的向量
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
        List<Transform> context = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius, 1 << LayerMask.NameToLayer("Default"));
        bool isExistingTarget = false;
        bool isExistingEscapeTarget = false;
        foreach (Collider c in colliders)
        {
            //获取同类鱼
            if (c.gameObject.tag == Rank)
            {
                context.Add(c.transform);
            }
            //获取要跟随的鱼
            if (c.gameObject.tag == "Player")
            {
                var rank = FlockManager.Instance.controllingFlockRank;
                if (Rank.CompareTo(rank) > 0)
                {
                    isExistingTarget = true;
                    _target = c.gameObject;
                }
                else
                {
                    isExistingEscapeTarget = true;
                    Escape(c);
                }

            }
        }
        if (!isExistingTarget)
        {
            _target = null;
        }
        if (!isExistingEscapeTarget)
        {
            isEscaping = false;
        }
        return context;
    }


    //每间隔几秒换一个方向
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0, maxDirectionChangeTime + 1));
        //只有在闲逛状态下才随机改变方向
        if (!isFollowing || !isEscaping)
        {
            direction = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
        }
        StartCoroutine(ChangeDirection());
    }


    public void OnTriggerEnter(Collider other)
    {
        //先检查是不是玩家，再检查是否为鱼群中的鱼
        if (other.tag == "Player")
        {
            if (FlockManager.Instance.controllingFlockRank.CompareTo(myRankTag) < 0)
            {
                if (other.gameObject.layer == 0)
                {
                    //关闭之前初始化
                    CameraController.Instance.CamShake();
                    CameraController.Instance.CamZoomDecreaseStart();
                    var _flockAI = other.gameObject.GetComponent<FlockAI>();
                    _flockAI.isInFlock = false;
                    other.gameObject.layer = LayerMask.NameToLayer("Unflocked");
                    FlockManager.Instance.Flocks.Remove(other.transform);
                }
                biteParticle.Play();
                other.gameObject.GetComponent<RecycleGameobject>().Shutdown();
            }
            
        }
        else
        {
            if (other.tag.CompareTo(myRankTag) < 0 && other.isTrigger ==false)
            {
                biteParticle.Play();
                other.gameObject.GetComponent<RecycleGameobject>().Shutdown();
            }
        }
    }


    private void Escape(Collider other)
    {
        isEscaping = true;
        direction = transform.position - FlockManager.Instance.flockCenter;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
    }

}
