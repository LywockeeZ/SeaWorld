using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float idleSpeed = 1f;
    public float maxDirectionChangeTime = 5f;
    public float angleSpeed = 5f;
    Vector3 direction = new Vector3(1,0,0);
    bool isRotating = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {
        idle();
    }

    void idle()
    {
        Turn();
        transform.position += idleSpeed * new Vector3(direction.x, direction.y, 0).normalized * Time.deltaTime;
        
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
    //每间隔几秒换一个方向
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0, maxDirectionChangeTime + 1));
        direction = new Vector3(Random.Range(-1,2), Random.Range(-1,2), 0);
        StartCoroutine(ChangeDirection());
    }


}
