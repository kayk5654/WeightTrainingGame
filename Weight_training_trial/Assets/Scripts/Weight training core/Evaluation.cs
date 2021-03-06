﻿using System.Collections;
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
	public 	LineRenderer 		visualizationPath;
	public  ExercisePhase 		exPhase;

	// output
	public float scoreOfRep; // 0.0 to 1.0
	public float totalScore;

	// internal use
	public  float 			repPeriod = 4.2f;
	public 	bool 			peakOfReps = false;
	public  bool 			endOfReps = false;
	public	float 			nextPeakOfReps;
	public	float 			lastPeak;
	private Vector3 		currentVel;
	private Vector3 		prevVel;
	public 	Vector3 		repStartPos;
	public 	Vector3 		repEndPos;
	private Vector3			lastEndPos;
	private List<Vector3> 	path;

	private bool 			lastInitState = false;
	public  bool 			isExercising = false;
	public  bool 			detectPeak = true;
	private float 			averageDot;

	// Use this for initialization
	void Start () {
		visualizationPath.enabled = false;
	}

	public void initForm(PackedInfo[] _input){
		// initialize form, range of movement, etc.
		PackedInfo input = _input[0];

		if (path == null) {
			path = new List<Vector3> ();
		}

		// start capturing form
		if (inInit && !lastInitState) {
			formInitProcess.initPhase = 1;
			repStartPos = input.Transform.position;
			repEndPos = input.Transform.position;
			lastInitState = inInit;
		}

		// capture form
		if (inInit){
			path.Add (input.Transform.position);
			float dist1 = Vector3.Distance(repStartPos, repEndPos);
			float dist2 = Vector3.Distance (repStartPos, input.Transform.position);
			if (dist1 < dist2) {
				repEndPos = input.Transform.position;
			}
		}

		// end capturing form
		if (!inInit && lastInitState) {
			formInitProcess.initPhase = 2;
			visualizePath ();
			lastInitState = inInit;

		}

	}

	public void initNextPeakOfReps(){
		lastPeak = Time.fixedTime;
		nextPeakOfReps = lastPeak + repPeriod;
		isExercising = true;
	}

	public void analize(PackedInfo _input){

		currentVel = _input.Velocity;
		float diffVel = Vector3.Dot (currentVel, prevVel);
		float dotTh = 0.8f;

		float accTh = 0.01f;

		float distTh = Vector3.Distance(repStartPos, repEndPos) * 0.3f;
		float distFromStartPos = Vector3.Distance (_input.Transform.position, repStartPos);
		float distFromEndPos = Vector3.Distance (_input.Transform.position, repEndPos);


		if (detectPeak) { // if the tracker is going to the furthest point
			endOfReps = false;

			// detect peak of motion of a rep
			if (distFromStartPos > distTh && diffVel < dotTh) {
				//if (_input.Acceleration < accTh && distFromStartPos > distTh && diffVel < dotTh) {
				peakOfReps = true;
				lastEndPos = _input.Transform.position;
				Debug.Log ("peak of reps");
				detectPeak = false;
			} else {
				peakOfReps = false;
			}
		} else { // if the tracker is going back to the starting point
			peakOfReps = false;

			// detect end of rep
			if (distFromStartPos < distTh && diffVel < dotTh) {
				//if (_input.Acceleration < accTh && distFromStartPos < distTh && diffVel < dotTh) {
				endOfReps = true;
				Debug.Log ("end of reps");
				detectPeak = true;
			} else {
				endOfReps = false;
			}
		}

		Debug.Log ("diffVel: " + diffVel);
		Debug.Log ("dist from start pos: " + distFromStartPos + " distTh: " + distTh + " detectPeak: " + detectPeak);

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
		if (phase == 2 && inputArray != null && isExercising) {
			// for trial version, which use only head
			if (Time.fixedTime > lastPeak + 0.1f) { // ignore packed info for certain period after peak or end of reps
				PackedInfo input = inputArray [0];
				analize (input);
			}
		
			if (peakOfReps) {
				lastPeak = Time.fixedTime;
				peakOfReps = false;
			}

			// calculate the target period of the next rep
			if (endOfReps) {
				evaluate ();
				exPhase.addRepCount ();
				lastPeak = Time.fixedTime;
				nextPeakOfReps = lastPeak + repPeriod;
				endOfReps = false;
			}
		}
	}

	void visualizePath(){
		visualizationPath.enabled = true;
		visualizationPath.endWidth = 0.1f;
		visualizationPath.startWidth = 0.1f;
		visualizationPath.SetPositions (path.ToArray());
	}
}
