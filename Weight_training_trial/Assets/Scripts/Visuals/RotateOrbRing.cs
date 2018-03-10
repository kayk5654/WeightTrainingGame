using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOrbRing : MonoBehaviour {

	private Vector3 localAsis;

	// Use this for initialization
	void Start () {
		localAsis = transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		//Quaternion.
	}
}
