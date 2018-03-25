using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseButtonHandler : MonoBehaviour {

	// input
	public ClickableExerciseButton[] 	exercisebuttons;
	public ExerciseManagement 			exManager;
	private ExerciseData[] 				exerciseList;


	void OnEnable () {
		init ();
	}

	// initialization
	void init(){
		exerciseList = exManager.exercises;

		// assign exercise names for each buttons
		for (int i = 0; i < exerciseList.Length; i++) {

			//instantiate buttons if it's necessary
			if (i >= exercisebuttons.Length) {
				exercisebuttons[i] = Instantiate (exercisebuttons [0]);
			}

			exercisebuttons [i].enabled = true;
			exercisebuttons [i].init ();
			exercisebuttons [i].gameObject.GetComponentInChildren<Text> ().text = exerciseList [i].exerciseName;
			exercisebuttons [i].data = exerciseList [i];
		}

		// deactivate buttons which aren't assigned any exercises
		if (exerciseList.Length < exercisebuttons.Length){
			for (int j = exerciseList.Length; j < exercisebuttons.Length; j++) {
				exercisebuttons [j].deactivate ();
			}
		}
	}

	// set exercise info of selected exercise on this class
	public void AssignSelectedParameters(ExerciseData _selectedData){
		exManager.exerciseId = _selectedData.exerciseId;
		exManager.typeOfTracker = _selectedData.typeOfTracker;
	}
}
