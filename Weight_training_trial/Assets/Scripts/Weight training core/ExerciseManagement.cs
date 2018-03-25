using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExerciseManagement : MonoBehaviour {

	// output
	public int 			repsNumber;
	public int 			setNumber;
	public float 		suggestedWeight = 0f;
	public int 			exerciseId;
	public float 		interval;
	public int[] 		typeOfTracker; // 0:head, 1:right hand, 2:left hand

	// internal use
	public ExerciseData[] exercises;

	void Awake () {
		
		// read data from json in the resources folder
		string filepath = "exerciseData";
		TextAsset[] exerciseJsons = Resources.LoadAll<TextAsset> (filepath);

		// create array of exercise data from json
		exercises = new ExerciseData[exerciseJsons.Length];

		for (int i = 0; i < exerciseJsons.Length; i++) {
			string jsonString = exerciseJsons [i].ToString();
			exercises[i] = JsonUtility.FromJson<ExerciseData>(jsonString);
		}
	}

	// when the player leaves menu level and load training level, this passes temporal file of selected exercise
	public void ExportTempExerciseData(){
		SelectedExercise selectedExercise = new SelectedExercise (repsNumber, setNumber, suggestedWeight, exerciseId, interval, typeOfTracker);
		string tempData = JsonUtility.ToJson (selectedExercise);

		string filepath = Application.temporaryCachePath + "selectedExercise.json";
		File.WriteAllText (filepath, tempData);
	}
}
