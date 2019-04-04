using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public string Rank;
    public float idleSpeed = 1f;
    public float flockSpeed = 1f;
    public float angleSpeed = 5f;
    public bool CanMove = true;
    public bool isInFlock = false;
    [Range(1, 50)]
    public float neighborRadius = 1f;
    public float maxDirectionChangeTime = 5f;


    Vector3 Direction = new Vector3(1,0,0);
    Vector3 mousePos = Vector3.zero;
    //Vector3 playerPos = Vector3.zero;
    bool isRotating = false;
    //float squareNeighborRadius;


    void Start()
    {
        //squareNeighborRadius = neighborRadius * neighborRadius;
        StartCoroutine(ChangeDirection());
    }


    void Update()
    {
        if (isInFlock)
        {
            Turn();
            if (CanMove)
            {
                Move(flockSpeed);
            }
        }
        else
        {
            Idle();
        }

    }


    public void Idle()
    {
        IdleTurn();
        Move(idleSpeed);

    }


    public void Move(float speed)
    {
        transform.position += speed * new Vector3(Direction.x,Direction.y,0).normalized * Time.deltaTime;
    }


    public void Turn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //如果是正交摄像机用以下代码
            //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Mask");
            RaycastHit hitInfo;          
            if (Physics.Raycast(ray, out hitInfo, 200, mask))
            {
                mousePos = hitInfo.point;
            }

            Direction = mousePos - transform.position;
            isRotating = true;
        }


        Vector3 _direction = new Vector3(Direction.x, Direction.y, 0);
 
        if (isRotating)
        {
            //计算出要旋转到目标方向的矩阵
            Quaternion rotate = Quaternion.LookRotation(_direction, Vector3.up);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, angleSpeed * Time.deltaTime);
            if (Vector3.Angle(_direction,transform.forward) < 0.01f)
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


        List<Transform> GetNearbyObjects()
        {
            List<Transform> context = new List<Transform>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius);
            foreach (Collider c in colliders)
            {

                //获取要跟随的鱼
                if (c.gameObject.tag == "PlayerFlock")
                {
                    context.Add(c.transform);

                }
            }
            return context;
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
