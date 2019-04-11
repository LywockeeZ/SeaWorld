using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    //-----------------------------------------------------------------------------
    // Data
    //-----------------------------------------------------------------------------
    public Entity templatePrefab = null;
    public GameObject shoalBounds;

    public float separationWeight = 0.75f;
    public float alignmentWeight = 0.8f;
    public float cohesionWeight = 0.7f;

    public bool moveShoal = false;

    public List<Entity> theFlock = new List<Entity>();

    public static App instance = null;

    public int numberOfEntities = 200;
    private int fishCount;

    private float speedCoefficient = 1.0f;
    private float maxSpeed = 0.3f;
    public GameObject finalPositionObject;
    private Vector3 finalPosition;
    private Vector3 direction;
    //-----------------------------------------------------------------------------
    // Functions
    //-----------------------------------------------------------------------------
    void Start ()
    {
        instance = this;
        fishCount = 0;
        StartCoroutine(InstantiateFlock());
        finalPosition = finalPositionObject.transform.position;
        direction = (finalPosition - transform.position).normalized;
    }
    void FixedUpdate()
    {
        if (moveShoal == true)
        {
            float distance = Vector3.Distance(finalPosition, transform.position); // Get distance to target
            float speed = Mathf.Clamp(distance * speedCoefficient, 0f, maxSpeed);
            GetComponent<Rigidbody>().velocity = direction * speed;
        }
    }
    //-----------------------------------------------------------------------------
    IEnumerator InstantiateFlock()
    {
        while (fishCount < numberOfEntities)
        {
            Entity flockEntity = Instantiate(templatePrefab, new Vector3(Random.Range(shoalBounds.transform.position.x - 20.0f, shoalBounds.transform.position.x + 20.0f), Random.Range(shoalBounds.transform.position.y - 5.0f, shoalBounds.transform.position.y + 5.0f), Random.Range(shoalBounds.transform.position.z - 20.0f, shoalBounds.transform.position.z + 20.0f)), templatePrefab.transform.rotation);
            flockEntity.transform.parent = gameObject.transform;
            flockEntity.SetID(fishCount);
            flockEntity.SetShoalBounds(shoalBounds);
            theFlock.Add(flockEntity);
            fishCount++;
            yield return null;
        }
    }
}
