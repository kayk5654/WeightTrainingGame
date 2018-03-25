using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutText : MonoBehaviour {

	public int 		sets;
	public int 		reps;
	public Text 	text;
	public ExercisePhase exPhase;

	// initialization
	void OnEnable () {
		sets = exPhase.setCount;
		reps = exPhase.repCount;
	}

	// update reps
	void Update () {
		reps = exPhase.repCount;
		string textString = reps + " reps\n" + sets + " sets";
		text.text = textString;
	}
}
