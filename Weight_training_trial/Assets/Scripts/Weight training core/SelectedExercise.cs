using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SelectedExercise {
	// this is the temporal data of selected exercise to pass between two scenes


	public int 			repsNumber;
	public int 			setNumber;
	public float 		suggestedWeight;
	public int 			exerciseId;
	public float 		interval;
	public int[] 		typeOfTracker;

	public SelectedExercise(int _reps, int _sets, float _weight, int _id, float _interval, int[] _trackers){
		repsNumber = _reps;
		setNumber = _sets;
		suggestedWeight = _weight;
		exerciseId = _id;
		interval = _interval;
		typeOfTracker = _trackers;
	}
}
