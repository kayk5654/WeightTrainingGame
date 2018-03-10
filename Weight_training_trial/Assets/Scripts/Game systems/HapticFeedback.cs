using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticFeedback : MonoBehaviour {

	// input
	public 		enum 		Controller {left, right};
	public 		Controller 	controller;
	public 		DeviceInfoGetter device;

	// internal use
	protected bool RHandIsPrimary = true;

	protected virtual void OnCollisionEnter (Collision other) {
		
	}
	
	protected virtual void OnCollisionExit (Collision other) {
		
	}

	public virtual void UIHaptics(){
		
	}
}
