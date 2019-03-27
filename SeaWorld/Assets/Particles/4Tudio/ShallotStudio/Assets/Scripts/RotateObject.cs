using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	public GameObject rotateObj;

	public Vector3 mySpeed;
	
	// Update is called once per frame
	void Update () {

		rotateObj.transform.Rotate (mySpeed);
		
	}
}
