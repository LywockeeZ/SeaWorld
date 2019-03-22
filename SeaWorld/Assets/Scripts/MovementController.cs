using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float Speed = 1f;
    public float angleSpeed = 5f;
    Vector3 Direction = new Vector3(1,0,0);
    Vector2 playerPos = Vector2.zero;
    bool isRotating = false;
    void Start()
    {
    }

    void Update()
    {
        Turn();
        Move();
    }

    public void Move()
    {
        transform.position += Speed * new Vector3(Direction.x,Direction.y,0).normalized * Time.deltaTime;
    }

    public void Turn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0) - transform.position);
            isRotating = true;
        }
        Vector3 _direction = new Vector3(Direction.x, Direction.y, 0);
        Quaternion rotate = Quaternion.LookRotation(_direction, Vector3.up);
        if (isRotating)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, angleSpeed * Time.deltaTime);
            if (Vector3.Angle(_direction,transform.forward) < 0.01f)
            {
                isRotating = false;
            }
        }
        
        
    }
}
