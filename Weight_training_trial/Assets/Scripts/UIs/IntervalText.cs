using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntervalText : MonoBehaviour {

	public int 		sets;
	public float 	interval;
	public Text 	text;
	public ExercisePhase exPhase;

	void OnEnable () {
		sets = exPhase.setCount;
		interval = exPhase.interval;
	}
	
	// Update is called once per frame
	void Update () {
		interval = exPhase.interval * 60f - exPhase.intervalTime;
		string textString = "You've finished " + sets + " set!\nThe next set will start in " + interval.ToString("F1") + " seconds.";
		text.text = textString;
	}
}
