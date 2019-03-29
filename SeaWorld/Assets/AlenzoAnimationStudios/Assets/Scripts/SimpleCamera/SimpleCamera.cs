using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleFixedCamera))]
[RequireComponent(typeof(SimpleFreeCamera))]

public class SimpleCamera : MonoBehaviour
{
    private SimpleFixedCamera simpleFixCamera;
    private SimpleFreeCamera simpleFreeCamera;

    private void Start()
    {
        simpleFixCamera = GetComponent<SimpleFixedCamera>();
        simpleFixCamera.enabled = true;
        simpleFreeCamera = GetComponent<SimpleFreeCamera>();
        simpleFreeCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            simpleFixCamera.enabled = !simpleFixCamera.enabled;
            simpleFreeCamera.enabled = !simpleFreeCamera.enabled;
        }	
	}
}
