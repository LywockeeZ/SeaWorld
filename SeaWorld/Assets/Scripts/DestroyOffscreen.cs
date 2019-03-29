using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    public float maxDistance = 45f;
    public bool canShutDown = true;
    RecycleGameobject myRecycle;
    // Start is called before the first frame update
    void Start()
    {
        myRecycle = GetComponent<RecycleGameobject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, FlockManager.Instance.flockCenter) > maxDistance)
        {
            if (canShutDown)
            {
                OnOutOfBounds();
            }
        }
    }

    public void OnOutOfBounds()
    {
        myRecycle.Shutdown();
    }
}
