using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExerciseData {

	public int 		exerciseId;
	public string 	exerciseName;
	public int[] 	typeOfTracker; // 0:head, 1:right hand, 2:left hand
	public float[] 	valueMin; //0:weight, 1:sets, 2:reps 
	public float[] 	valueMax; //0:weight, 1:sets, 2:reps 
	public float[] 	step; //0:weight, 1:sets, 2:reps 
}
