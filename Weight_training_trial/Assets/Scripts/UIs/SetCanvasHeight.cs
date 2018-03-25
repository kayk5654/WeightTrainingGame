using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasHeight : MonoBehaviour {

	public 	DeviceInfoGetter 	device;
	public  bool 				resetHeight;

	// initialization
	void OnEnable () {
		transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
	}

	// set the hight of the canvas based on the hight of the head
	void reset(){
		transform.position = new Vector3(transform.position.x, device.head.transform.position.y - 0.2f, transform.position.z);
	}

	void Update(){
		if (resetHeight) {
			reset ();
		}
	}
}
