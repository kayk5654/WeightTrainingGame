using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour {

	// output
	private int exerciseId;

	//internal use
	public 	GameObject 		core;	// Use this for initialization
	private List<string> 	exercises;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// if the player selected exercise
		ExerciseManagement exManage = core.GetComponent<ExerciseManagement> ();
		exManage.exerciseId = exerciseId;
		//exManage.AssignData ();
	}
}
