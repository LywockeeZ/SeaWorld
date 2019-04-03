using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorStateControllor : MonoBehaviour
{
    public bool canShutDown = true;
    float maxDistance;
    float minDistance;
    Generator myGenerator;
    FlockManager flockManager;
    // Start is called before the first frame update
    void Start()
    {
        flockManager = FlockManager.Instance;
        maxDistance = flockManager.shutDownBoundary;
        minDistance = flockManager.visualBoundary;
        myGenerator = GetComponent<Generator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, flockManager.flockCenter + flockManager.visualBoundaryOffset) > maxDistance ||
            Vector3.Distance(transform.position, flockManager.flockCenter + flockManager.visualBoundaryOffset) - myGenerator.GenerateRadius < minDistance)
        {
            if (canShutDown)
            {
                OnOutOfBounds();
            }
        }
        else
        {
            InTheBounds();
        }
    }

    public void OnOutOfBounds()
    {
        myGenerator.active = false;
    }

    public void InTheBounds()
    {
        myGenerator.active = true;
    }
}
