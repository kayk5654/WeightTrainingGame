using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftHapticFeedback : HapticFeedback {

	// internal use
	private OVRHapticsClip 	basicHapticsClip;
	private Vector3 		lastContactPoint;
	private bool 			shortHapticsTrigger = false;

	void Start(){
		byte[] sample = new byte[8];
		for (int i = 0; i < sample.Length; i++){
			sample[i] = (byte)(128 * (i % 2));
		}
		basicHapticsClip = new OVRHapticsClip (sample, sample.Length);
	}

	protected override void OnCollisionEnter (Collision other) {
		if (controller == Controller.left) {
			OVRHaptics.LeftChannel.Mix (basicHapticsClip);
		} else {
			OVRHaptics.RightChannel.Mix (basicHapticsClip);
		}
	}
	
	protected override void OnCollisionExit (Collision other) {
		if (controller == Controller.left) {

		} else {

		}
	}

	public override void UIHaptics(){
		shortHapticsTrigger = true;

		if (shortHapticsTrigger) {
			if (controller == Controller.right && device.RHandIsPrimary) {
				OVRHaptics.RightChannel.Mix (basicHapticsClip);
			} else if (controller == Controller.left && !device.RHandIsPrimary) {
				OVRHaptics.LeftChannel.Mix (basicHapticsClip);
			}
			shortHapticsTrigger = false;
		}
	}
}
