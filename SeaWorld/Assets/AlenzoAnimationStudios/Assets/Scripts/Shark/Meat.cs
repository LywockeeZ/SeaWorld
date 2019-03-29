using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Meat : MonoBehaviour
{
    [SerializeField] private float distToDestroyBite = 0.8f;
    [SerializeField] private float distToHitSpeed = 0.4f;
    public int meatPoint = 5;

    private bool wasBittenFlag = false;
    private Rigidbody meatRb;
    private GameObject biteObj;

    void Start ()
    {
        meatRb = GetComponent<Rigidbody>();
        meatRb.mass = 1;
        meatRb.drag = 1;
        meatRb.angularDrag = 1;
        meatRb.useGravity = false;
        meatRb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collObj = other.gameObject;
        if (other.gameObject.name == "MeatDetector")
        {
            biteObj = other.gameObject;
            wasBittenFlag = true;
        }
    }

    private void FixedUpdate()
    {
        if (wasBittenFlag)
        {
            MoveMeatToBite();
        }
    }

    private void MoveMeatToBite()
    {
        Vector3 desirePosition = Vector3.MoveTowards(transform.position, biteObj.transform.position, distToHitSpeed);
        meatRb.MovePosition(desirePosition);

        if (Vector3.Distance(transform.position, biteObj.transform.position) < distToDestroyBite)
            Destroy(gameObject);
    }
}
