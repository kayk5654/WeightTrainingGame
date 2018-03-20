using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiftInfoGetter : DeviceInfoGetter {

	// input
	public OVRInput.Controller 	controller;

	// internal use
	private OVRHapticsClip basicHapticsClip;

	protected override void Start () {
		controller = OVRInput.GetConnectedControllers ();

		byte[] sample = new byte[8];
		for (int i = 0; i < sample.Length; i++){
			sample[i] = 128;
		}
		basicHapticsClip = new OVRHapticsClip (sample, sample.Length);


		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Training") {
			base.Start ();
		}
	}

	protected override void assignControllerAction(){

		// switch controller for operation
		if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)){
			RHandIsPrimary = true;
			OVRHaptics.RightChannel.Mix (basicHapticsClip);
		}
		if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger)){
			RHandIsPrimary = false;
			OVRHaptics.LeftChannel.Mix (basicHapticsClip);
		}

		// send controller status to controller handler
		if (RHandIsPrimary) {
			controllerHandler.selecting = OVRInput.GetUp (OVRInput.RawButton.RIndexTrigger);
			controllerHandler.scrollUp = OVRInput.GetDown (OVRInput.RawButton.RThumbstickUp);
			controllerHandler.scrollDown = OVRInput.GetDown (OVRInput.RawButton.RThumbstickDown);
			controllerHandler.scrollDir = OVRInput.Get (OVRInput.RawAxis2D.RThumbstick);
			controllerHandler.forwardVec = rightHand.transform.forward;
			controllerHandler.rayOrigin = rightHand.transform.position;
		} else {
			controllerHandler.selecting = OVRInput.GetUp (OVRInput.RawButton.LIndexTrigger);
			controllerHandler.scrollUp = OVRInput.GetDown (OVRInput.RawButton.LThumbstickUp);
			controllerHandler.scrollDown = OVRInput.GetDown (OVRInput.RawButton.LThumbstickDown);
			controllerHandler.scrollDir = OVRInput.Get (OVRInput.RawAxis2D.LThumbstick);
			controllerHandler.forwardVec = leftHand.transform.forward;
			controllerHandler.rayOrigin = leftHand.transform.position;
		}

		// activate / deactivate interrupt menu in training scene
		if (OVRInput.GetDown (OVRInput.RawButton.Start) && controllerHandler.interruptCanvas != null) {
			controllerHandler.interruptMenuActivated = !controllerHandler.interruptMenuActivated;
		}

		// reset height of the ui panel
		foreach (SetCanvasHeight canvas in canvasHeight) {
			if (canvas.transform.gameObject.activeSelf) {
				canvas.resetHeight = OVRInput.GetDown (OVRInput.RawButton.Y);
			}
		}
	}

	protected override void FixedUpdate () {

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Menu") {
			return;
		}

		if (phase == 0)
			return;

		base.getPackedInfo ();

		// send packed info to Evaluation class
		if (phase == 1) {
			evaluation.initForm (packedInfo);
		} else if (phase == 2) {
			evaluation.inputArray = packedInfo;
		}

	}

	protected override void Update(){

		assignControllerAction ();

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Training") {
			//debug ();
		}
	}

	// for debugging
	void debug(){
		

		string debugText = "";

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Training") {
			debugText = "phase: " + phase + "\n" + "acceleration: " + packedInfo [0].Acceleration + "\n" + "velocity: " + packedInfo[0].Velocity + "\n" 
			                   + "start position: " + evaluation.repStartPos + "\n" + "end position: " + evaluation.repEndPos + "\n"
			                   + "peak of rep: " + evaluation.peakOfReps + "\n" + "diff from startpos: " + Vector3.Distance (evaluation.repStartPos, head.transform.position) + "\n"
			                   + "score of rep: " + evaluation.scoreOfRep;
		} else {
			debugText = "phase: " + phase + "\n" + "acceleration: ";
		}
		GameObject.Find ("Debug Text").GetComponent<Text> ().text = debugText;
	}
}