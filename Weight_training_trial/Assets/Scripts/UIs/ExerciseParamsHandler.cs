using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseParamsHandler : MonoBehaviour {

	// input
	public ParamInputField[] 	parameters; // 0->weight, 1->sets, 2->reps, 3->interval
	public ExerciseManagement 	exManager;
	public ExerciseData selectedExercise;

	void OnEnable(){
		init ();
	}

	void init(){
		// get selected exercise
		selectedExercise = exManager.exercises [exManager.exerciseId];

		// read range and steps of each parameters
		for (int i = 0; i < parameters.Length; i++){
			if (i == 3) {
				parameters [i].valueMin = 0.5f;
				parameters [i].valueMax = 2f;
				parameters [i].step = 0.1f;
				parameters [i].init (selectedExercise.exerciseId);
				parameters [i].display ();
				return;
			}

			parameters[i].valueMin = selectedExercise.valueMin[i];
			parameters [i].valueMax = selectedExercise.valueMax [i];
			parameters [i].step = selectedExercise.step [i];
			parameters [i].init (selectedExercise.exerciseId);
			parameters [i].display ();
		}
	}

	public void AssignSelectedParameters(){
		exManager.suggestedWeight = parameters[0].selectedValue;
		exManager.setNumber = Mathf.RoundToInt(parameters[1].selectedValue);
		exManager.repsNumber = Mathf.RoundToInt(parameters[2].selectedValue);
		exManager.interval = parameters[3].selectedValue;
	}
}
