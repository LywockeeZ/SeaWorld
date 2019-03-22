using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float idleSpeed = 0.5f;
    public float directionChangeTime = 1f;
    Vector3 direction = new Vector3(1,0,0);
    
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
        transform.position += idleSpeed * new Vector3(direction.x, direction.y, 0).normalized * Time.deltaTime;
    }

    //每间隔几秒换一个方向
    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(directionChangeTime);
        direction = new Vector3(Random.Range(-1,2), Random.Range(-1,2), 0);
        Debug.Log(direction);
        StartCoroutine(ChangeDirection());
    }
}
