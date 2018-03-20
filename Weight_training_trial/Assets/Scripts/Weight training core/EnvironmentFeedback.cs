using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentFeedback : MonoBehaviour {

	// input
	public 	GameObject 			environment;
	public 	enum FeedbackType 	{scale = 0, moveUp = 1};
	public 	FeedbackType 		feedbackType = 0;
	public 	float 				effectFactor = 0.5f;
	public 	Evaluation 			evaluation;
	public 	ExercisePhase 		exPhase;

	// internal use
	private Vector3 initScale;
	private Vector3 initPosition;
	private Vector3 targetScale;
	private Vector3 targetPosition;
	private Vector3 beginScale;
	private Vector3 beginPosition;
	private float 	phase;

	void Start(){
		if (SceneManager.GetActiveScene ().name == "Menu") {
			return;
		}

		init ();
	}

	void init(){
		if (feedbackType == FeedbackType.scale) {
			initScale = environment.transform.localScale;
			targetScale = initScale;
		} else {
			initPosition = environment.transform.position;
			targetPosition = initPosition;
		}
	}

	void Update () {

		if(OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp)){ // for testing purpose
			
			if (feedbackType == 0) {
				setTargetScale(exPhase.getScoreOfSet());
			} else {
				setTargetPosition();
			}

			Debug.Log ("feedback triggered");
		}

		if (feedbackType == FeedbackType.scale) {
			scale ();
		} else {
			moveUp ();
		}

		// for debugging purpose
		if (OVRInput.GetDown (OVRInput.RawButton.LThumbstickDown)) {
			reset ();
		}
	}

	void reset(){
		
		if (feedbackType == FeedbackType.scale) {
			environment.transform.localScale = initScale;
			targetScale = initScale;
		} else {
			environment.transform.position = initPosition;
			targetPosition = initPosition;
		}
		
	}

	public void setTarget(){
		if (feedbackType == 0) {
			setTargetScale(exPhase.getScoreOfSet());
		} else {
			setTargetPosition();
		}

		Debug.Log ("feedback triggered");
	}

	void setTargetScale(float _scoreOfSet = 0f){
		targetScale = environment.transform.localScale * (1f - (effectFactor * _scoreOfSet));
		beginScale = environment.transform.localScale;
		phase = 0f;
	}

	void setTargetPosition(){
		Vector3 currentPosition = environment.transform.position;
		targetPosition = new Vector3 (currentPosition.x, currentPosition.y * (1-effectFactor), currentPosition.z);
		beginPosition = environment.transform.position;
		phase = 0f;
	}

	void scale (){
		if (environment.transform.localScale.x > targetScale.x) {
			//environment.transform.localScale *= (1f - 0.005f);


			phase += 0.005f;
			float modifiedPhase = Mathf.SmoothStep(0f, 1f, phase);
			environment.transform.localScale = Vector3.Lerp (beginScale, targetScale, modifiedPhase);

			if (phase > 1f) {
				phase = 1f;
			}
			
		} else {
			environment.transform.localScale = targetScale;
		}
	}

	void moveUp (){
		if (environment.transform.position.y > targetPosition.y) {
			//environment.transform.position -= new Vector3(0, 0.005f, 0);


			phase += 0.005f;
			float modifiedPhase = Mathf.SmoothStep(0f, 1f, phase);
			environment.transform.localScale = Vector3.Lerp (beginPosition, targetPosition, modifiedPhase);

			if (phase > 1f) {
				phase = 1f;
			}

		} else {
			environment.transform.position = targetPosition;
		}
	}
}
