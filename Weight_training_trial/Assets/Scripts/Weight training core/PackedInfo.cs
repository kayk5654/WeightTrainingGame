using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackedInfo {

	public Transform 	Transform;
	public Vector3 		Velocity = Vector3.zero;
	public float 		Acceleration = 0.0f;

	private Vector3 	lastPosition = Vector3.zero;
	private List<float> accList;
	private List<Vector3> velList;

	public PackedInfo(){
		accList = new List<float> ();
		velList = new List<Vector3> ();
	}

	public void update(){
		Vector3 rawVel = Transform.position - lastPosition;

		// use moving average for calculation of acceleration
		accList.Add(rawVel.magnitude);

		if (accList.Count > 5) {
			accList.RemoveAt (0);
		}

		float accSum = 0;
		foreach (float acc in accList) {
			accSum += acc;
		}

		Acceleration = accSum / accList.Count;


		// use moving average for calculation of velocity

		velList.Add (rawVel.normalized);

		if (velList.Count > 2) {
			velList.RemoveAt (0);
		}

		Vector3 velSum = Vector3.zero;
		foreach (Vector3 vel in velList) {
			velSum = velSum + vel;
		}
		Velocity = velSum.normalized;

		lastPosition = Transform.position;
	}
}
