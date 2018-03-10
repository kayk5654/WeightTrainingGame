using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluation : MonoBehaviour {

	// input
	public 	int 				exerciseId; // set by UI handler
	public 	bool 				initButtonDown = false;
	public 	PackedInfo[] 		inputArray;
	public 	DeviceInfoGetter 	device;
	public 	int[]			 	typeOfTracker;
	public	int 				phase;
	public 	FormInitProcess 	formInitProcess;
	public	bool 				inInit = false;

	// output
	public float scoreOfRep; // 0.0 to 1.0
	public float totalScore;

	// internal use
	private float 			repPeriod = 4.2f;
	public 	bool 			peakOfReps = false;
	private bool 			endOfReps = false;
	private	float 			nextPeakOfReps;
	private float 			lastRep;
	private Vector3 		currentVel;
	private Vector3 		prevVel;
	public 	Vector3 		repStartPos;
	public 	Vector3 		repEndPos;
	private Vector3			lastEndPos;
	private List<Vector3> 	path;

	private bool 			lastInitState = false;

	// Use this for initialization
	void Start () {
	}

	public void initForm(PackedInfo[] _input){
		// initialize form, range of movement, etc.
		PackedInfo input = _input[0];

		if (path == null) {
			path = new List<Vector3> ();
		}

		if (inInit && !lastInitState) {
			formInitProcess.initPhase = 1;
			repStartPos = input.Transform.position;
			repEndPos = input.Transform.position;
			lastInitState = inInit;
		}

		if (inInit){
			//path.Add (input.Transform.position);
			float dist1 = Vector3.Distance(repStartPos, repEndPos);
			float dist2 = Vector3.Distance (repStartPos, input.Transform.position);
			if (dist1 < dist2) {
				repEndPos = input.Transform.position;
			}
			Debug.Log("inInit");
		}

		if (!inInit && lastInitState) {
			formInitProcess.initPhase = 2;
			lastInitState = inInit;
			//visualizePath ();
		}

	}

	public void initNextPeakOfReps(){
		nextPeakOfReps = Time.fixedTime + repPeriod;
	}

	public void analize(PackedInfo _input){

		currentVel = _input.Velocity;
		float diffVel = Vector3.Dot (currentVel, prevVel);
		float dotTh = 0.6f;

		float accTh = 0.01f;

		float distTh = 0.3f;
		float distFromStartPos = Vector3.Distance (_input.Transform.position, repStartPos);
		float distFromEndPos = Vector3.Distance (_input.Transform.position, repEndPos);

		// detect peak of motion of a rep
		if (_input.Acceleration < accTh && distFromStartPos > distTh && diffVel < dotTh) {
			peakOfReps = true;
			lastEndPos = _input.Transform.position;
		} else {
			peakOfReps = false;
		}

		// detect end of rep
		if (_input.Acceleration < accTh && distFromStartPos < distTh && diffVel < dotTh) {
			endOfReps = true;
		} else {
			endOfReps = false;
		}
		Debug.Log (diffVel);
		prevVel = currentVel;

	}

	// evaluate rep by effectivity of exercise. For the first editin, it's time based.
	void evaluate (){
		float maxDiffTime = 1.5f;
		float diffTime = Mathf.Clamp(Mathf.Abs (Time.fixedTime - nextPeakOfReps), 0f, maxDiffTime);

		float maxDist = Vector3.Distance (repStartPos, repEndPos);
		float diffDist = Mathf.Clamp(Vector3.Distance (lastEndPos, repStartPos), 0f, maxDist * 0.5f);

		scoreOfRep = ((maxDiffTime-diffTime) + (maxDist-diffDist))/ (maxDiffTime + maxDist);
	}

	void FixedUpdate () {

		if (phase != 2 || inputArray == null) {
			return;
		}

		// for trial version, which use only head
		if (Time.fixedTime > nextPeakOfReps + 0.5f) {
			PackedInfo input = inputArray [0];
			analize (input);
		}

		// calculate next ideal peak of rep
		if (peakOfReps) {
			evaluate ();
		}

		if (endOfReps) {
			nextPeakOfReps = Time.fixedTime + repPeriod;
		}
	}

	void debug(){
		
	}

	void visualizePath(){
		LineRenderer line = transform.gameObject.AddComponent<LineRenderer> ();
		line.endWidth = 0.1f;
		line.startWidth = 0.1f;
		line.startColor = Color.red;
		line.endColor = Color.red;
		line.SetPositions (path.ToArray());
	}
}
