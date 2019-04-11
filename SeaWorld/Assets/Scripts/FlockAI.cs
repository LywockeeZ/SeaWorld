using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAI : MonoBehaviour
{
    public string Rank;
    public float idleSpeed = 1f;
    public float angleSpeed = 5f;
    public bool CanMove = false;
    public bool isInFlock = false;
    [Range(0, 50)]
    public float neighborRadius = 1f;
    public Vector3 offset = Vector3.zero;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    public float maxDirectionChangeTime = 5f;
    public ParticleSystem biteParticle;

    Vector3 Direction = new Vector3(1, 0, 0);
    bool isRotating = false;
    List<Transform> flockList;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    //cohensionMove需要的变量
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;
    public Animator UIanimator;
    private SharkController sharkController;
    private float camDistence;


    //private void Awake()
    //{
    //    CanMove = false;
    //    isInFlock = false;
    //    GetComponent<DestroyOffscreen>().canShutDown = false;
    //}
    void Start()
    {
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        flockList = FlockManager.Instance.Flocks;
        StartCoroutine(ChangeDirection());
        sharkController = GetComponent<SharkController>();
    }


    void FixedUpdate()
    {
        if (CanMove)
        {
            if (!isInFlock)
            {
                Idle();
            }
            else
            {
           
                //FlockMove();
                if (CheckState())
                {
                    CheckAround();
                    
                }


            }
        }

 
    }


    public void Idle()
    {
        //IdleTurn();
        Turn();
        Move(idleSpeed );
    }

    public void FlockMove()
    {
        Turn();
        //基本速度+随时间增加的难度速度
        MoveAddConhesion(FlockManager.Instance.moveSpeed + GameManager.Instance.IncreaseSpeed);
        
    }


    public void Move(float speed)
    {
        transform.position += (speed) * new Vector3(Direction.x, Direction.y, 0).normalized * Time.deltaTime;
    }


    public void MoveAddConhesion(float speed)
    {
        Vector3 cohesion = CalculateCohesion();
        transform.position += (speed - 4) * new Vector3(Direction.x, Direction.y, 0).normalized * Time.deltaTime + 0.05f * cohesion;
    }


    public void Turn()
    {
        if (isInFlock)
        {
            if (Input.GetMouseButton(0))
            {
                isRotating = true;
                Direction = FlockManager.Instance.GetDirection();
            }
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


    //public void IdleTurn()
    //{
    //    Vector3 _direction = new Vector3(Direction.x, Direction.y, 0);
    //    isRotating = true;

    //    if (isRotating)
    //    {
    //        //计算出要旋转到目标方向的矩阵
    //        if (_direction.magnitude != 0)
    //        {
    //            Quaternion.LookRotation()
    //            Quaternion rotate = Quaternion.LookRotation(_direction, Vector3.up);
    //            transform.rotation = Quaternion.Slerp(transform.localRotation, rotate, angleSpeed * Time.deltaTime);
    //        }
    //        if (Vector3.Angle(_direction, transform.forward) < 0.01f)
    //        {
    //            isRotating = false;
    //        }
    //    }
    //}


    public Vector2 CalculateCohesion()
    {
        //如果没有邻居，就不用调整
        if (flockList.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 cohesionMove = FlockManager.Instance.flockCenter;

        //对当前位置计算一下偏差
        cohesionMove -= (Vector2)transform.position;
        cohesionMove = Vector2.SmoothDamp(transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }


    //public Vector2 CalculateAvoidance()
    //{
    //    if (flockList.Count == 0)
    //    {
    //        return Vector2.zero;
    //    }

    //    //若需要分离，则执行分离操作
    //    Vector2 avoidanceMove = Vector2.zero;
    //    //需要分离的邻居的个数
    //    int nAvoid = 0;
    //    foreach (Transform item in flockList)
    //    {
    //        if (Vector2.SqrMagnitude(item.position - transform.position) < SquareAvoidanceRadius)
    //        {
    //            nAvoid++;
    //            avoidanceMove += (Vector2)(transform.position - item.position);
    //            //Debug.Log(avoidanceMove);
    //            //avoidanceMove = Vector2.SmoothDamp(agent.transform.forward, avoidanceMove, ref currentVelocity, agentSmoothTime);
    //            //avoidanceMove = Vector2.Lerp(agent.transform.forward, avoidanceMove, agentSmoothTime);
    //        }

    //    }
    //    if (nAvoid > 0)
    //    {
    //        avoidanceMove /= nAvoid;
    //    }
    //    return avoidanceMove;
    //}


    public void CheckAround()
    {
        //List<Transform> context = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position + offset, neighborRadius, 1 << LayerMask.NameToLayer("Unflocked"));

        foreach (Collider c in colliders)
        {
            
            //如果在未聚集的层，则改为已聚集的层，防止重复探测
            if (c.gameObject.layer == LayerMask.NameToLayer("Unflocked"))
            {
                FlockManager.Instance.Flocks.Add(c.transform);
                c.gameObject.layer = 0;
                var _flockAI = c.gameObject.GetComponent<FlockAI>();               
                _flockAI.isInFlock = true;
                //使其与鱼群方向相同
                _flockAI.Direction = FlockManager.Instance.flockDirection;
                c.transform.rotation = FlockManager.Instance.flockRotation;
                //使他的位置沿半径方向缩回0.1
                //c.transform.position -= (c.transform.position - FlockManager.Instance.flockCenter).normalized * (Vector3.Distance(transform.position, FlockManager.Instance.flockCenter) - 0.95f);
                //CameraController.Instance.camAnimator.SetTrigger("CamZoom");
                CameraController.Instance.CamZoomIncreaseStart();
                _flockAI.UIanimator.SetTrigger("Flocked");
                GameManager.Instance.scores += FlockManager.Instance.Flocks.Count;
                var sharkcontroller = c.gameObject.GetComponent<SharkController>();
                CameraController.Instance.CamDistance -= 2;
                CameraController.Instance.offset = new Vector3(CameraController.Instance.offset.x, CameraController.Instance.offset.y + 1, CameraController.Instance.offset.z);
                sharkcontroller.yaw = FlockManager.Instance.yaw;
                sharkcontroller.pitch = FlockManager.Instance.pitch;
            }
        }
    }


    public bool CheckState()
    {
        if (true)
        {
            
        }
        return true;
    }


    //每间隔几秒换一个方向
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0, maxDirectionChangeTime + 1));
        //只有在闲逛状态下才随机改变方向
        if (!isInFlock)
        {
            isRotating = true;
            Direction = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
        }
        StartCoroutine(ChangeDirection());
    }


    private void OnTriggerEnter(Collider other)
    {
        //吃掉等级小的 && 防止碰到身体触发 && 防止吃自己人
        if (other.tag.CompareTo(Rank) < 0 && other.isTrigger == false && other.tag != "Player")
        {
            biteParticle.Play();
            other.gameObject.GetComponent<RecycleGameobject>().Shutdown();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + offset, neighborRadius);
 
    }
}
