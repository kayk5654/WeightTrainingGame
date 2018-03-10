using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutText : MonoBehaviour {

	public int 		sets;
	public int 		reps;
	public Text 	text;
	public ExercisePhase exPhase;

	void OnEnable () {
		sets = exPhase.setCount;
		reps = exPhase.repCount;
	}

	// Update is called once per frame
	void Update () {
		reps = exPhase.repCount;
		string textString = reps + " reps\n" + sets + " sets";
		text.text = textString;
	}
}
