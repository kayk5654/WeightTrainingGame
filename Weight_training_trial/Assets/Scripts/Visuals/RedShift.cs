using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShift : MonoBehaviour {

	public Renderer rend;
	public ExercisePhase exPhase;

	private Shader redshift;
	private float beginLevel;
	private float targetLevel;
	private float shiftPhase = 0f;
	private int lastRepCount;

	// initialization
	void Start () {
		redshift = Shader.Find ("WeightTrainingGame/RedShiftUnlit");

		if (rend.material.shader != redshift) {
			return;
		}

		beginLevel = rend.material.GetFloat ("_RedShadeLevel");
		targetLevel = rend.material.GetFloat ("_RedShadeLevel");
	}
	
	// update red shading
	void Update () {
		if (rend.material.shader != redshift) {
			return;
		}

		if (exPhase.repCount != lastRepCount) {
			setTargetLevel ();
			lastRepCount = exPhase.repCount;
		}

		shiftRedLevel ();

	}

	// set target level of red shade
	void setTargetLevel(){
		float step = 1f / (float)(exPhase.repsNumber - 1);
		beginLevel = rend.material.GetFloat ("_RedShadeLevel");

		if (beginLevel < 1f) {
			targetLevel = step * exPhase.repCount;
		} else {
			targetLevel = 0f;
		}
		shiftPhase = 0f;
	}

	// update red shading
	void shiftRedLevel(){
		rend.material.SetFloat("_RedShadeLevel", Mathf.Lerp (beginLevel, targetLevel, shiftPhase));
		shiftPhase += 0.01f;

		if (shiftPhase > 1f) {
			shiftPhase = 1f;
		}
	}
}
