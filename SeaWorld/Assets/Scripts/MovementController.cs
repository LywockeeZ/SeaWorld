using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float Speed = 1f;
    public float angleSpeed = 5f;
    public bool CanMove = true;
    Vector3 Direction = new Vector3(1,0,0);
    Vector3 mousePos = Vector3.zero;
    Vector3 playerPos = Vector3.zero;
    bool isRotating = false;
    void Start()
    {
    }

    void Update()
    {
        Turn();
        if (CanMove)
        {
            Move();
        }
    }

    public void Move()
    {
        transform.position += Speed * new Vector3(Direction.x,Direction.y,0).normalized * Time.deltaTime;
    }

    public void Turn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //如果是正交摄像机用以下代码
            //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << 10;
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
}
