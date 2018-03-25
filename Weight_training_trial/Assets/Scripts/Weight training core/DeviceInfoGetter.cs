using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInfoGetter : MonoBehaviour {

	//input
	public GameObject head;
	public GameObject rightHand;
	public GameObject leftHand;

	// output
	public bool 				peakOfReps;
	public Evaluation 			evaluation;
	public ControllerHandler 	controllerHandler;
	public bool 				RHandIsPrimary = true;
	public SetCanvasHeight[]		canvasHeight;

	//internal use
	public int phase = 0; // 0:idle, 1:initialize, 2:analize
	protected PackedInfo[] 	packedInfo;
	protected int[] 	typeOfTracker;

	// Use this for initialization
	protected virtual void Start () {
		initPackedInfo ();
	}

	void initPackedInfo(){
		typeOfTracker = evaluation.typeOfTracker;

		packedInfo = new PackedInfo[typeOfTracker.Length];

		for (int i = 0; i < packedInfo.Length; i++) {
			packedInfo [i] = new PackedInfo ();
		}
	}

	protected virtual void assignControllerAction(){
		
	}

	// get information from trackers
	protected virtual void getPackedInfo(){

		for (int i = 0; i < typeOfTracker.Length; i++) {

			if (typeOfTracker[i] == 0) {
				packedInfo[i].Transform = head.transform;
				packedInfo[i].update ();
			}

			if (typeOfTracker[i] == 1) {
				packedInfo[i].Transform = rightHand.transform;
				packedInfo[i].update ();
			}

			if (typeOfTracker[i] == 2) {
				packedInfo[i].Transform = leftHand.transform;
				packedInfo[i].update ();
			}
		}
	}

	protected virtual void Update () {
		// update input from controller for operation of UI
	}

	protected virtual void FixedUpdate(){
		// update status of trackers for evaluation of exercise
	}
}
