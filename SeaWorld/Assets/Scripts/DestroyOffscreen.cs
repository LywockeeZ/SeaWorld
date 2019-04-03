using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    public bool canShutDown = true;
    float maxDistance;
    RecycleGameobject myRecycle;
    FlockManager flockManager;
    // Start is called before the first frame update
    void Start()
    {
        flockManager = FlockManager.Instance;
        maxDistance = flockManager.shutDownBoundary;
        myRecycle = GetComponent<RecycleGameobject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, flockManager.flockCenter + flockManager.visualBoundaryOffset) > maxDistance)
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
