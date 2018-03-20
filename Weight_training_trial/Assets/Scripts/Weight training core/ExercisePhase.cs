using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ExercisePhase : MonoBehaviour {

	// input from temp file
	public  int		repsNumber;
	private int		setNumber;
	private float 	suggestedWeight;
	private int 	exerciseId;
	public	float 	interval;
	private int[] 	typeOfTracker;

	// input
	public DeviceInfoGetter	 	device;
	public PageTransition 		mainCanvasTransition;
	public ScaleOrb 			orb;
	public EnvironmentFeedback 	environment;
	public SetEndParticleEffect particles;

	// output
	public Evaluation 	evaluation;
	public int 			phase = 1;
	public Text 		scoreOnUI;

	// internal use
	public  int 			setCount = 0;
	public  int 			repCount = 0;
	private List<float> 	scores;
	public	bool 			inInterval = false;
	public  float 			intervalTime;

	void Awake () {
		ReadTempDataFromExerciseManagement ();
		AssignDataForEvaluation ();

		scores = new List<float> ();
	}

	void Start(){
		// display the first page
		mainCanvasTransition.transition (mainCanvasTransition.pages[0]);

		// initialize phase
		phase = 1;
		device.phase = phase;
		evaluation.phase = phase;
	}

	void ReadTempDataFromExerciseManagement(){
		// read temp file
		string filepath = Application.temporaryCachePath + "selectedExercise.json";
		string tempData = File.ReadAllText (filepath);

		// unpack temp file and assign its contents for variables of this class
		SelectedExercise selectedExercise = JsonUtility.FromJson<SelectedExercise> (tempData);

		repsNumber = selectedExercise.repsNumber;
		setNumber = selectedExercise.setNumber;
		suggestedWeight = selectedExercise.suggestedWeight;
		exerciseId = selectedExercise.exerciseId;
		interval = selectedExercise.interval;
		typeOfTracker = selectedExercise.typeOfTracker;

		// delete temp file
		File.Delete (filepath);

		// for debugging
		Debug.Log("repsNumber: " + repsNumber);
	}

	void AssignDataForEvaluation(){
		evaluation.exerciseId = exerciseId;
		evaluation.typeOfTracker = typeOfTracker;
	}

	public void updatePhase(){
		phase++;
		if (phase > 2) {
			phase = 0;
		}

		device.phase = phase;
		evaluation.phase = phase;
	}

	public void backToPrevPhase(){
		phase--;

		if (phase < 0) {
			phase = 0;
		}

		device.phase = phase;
		evaluation.phase = phase;
	}

	// count reps
	public void addRepCount(){
		repCount++;
		scores.Add (evaluation.scoreOfRep);
		orb.setTargetScale (evaluation.scoreOfRep);
	}

	void count(){
		// count interval
		if (inInterval) {
			
			intervalTime += Time.deltaTime;

			if (intervalTime > interval * 60f){
				evaluation.peakOfReps = false;
				evaluation.endOfReps = false;
				evaluation.detectPeak = true;
				evaluation.isExercising = true;
				evaluation.lastPeak = Time.fixedTime;
				evaluation.nextPeakOfReps = evaluation.lastPeak + evaluation.repPeriod;
				mainCanvasTransition.transition (mainCanvasTransition.pages [4]);
				inInterval = false;
			}
			return;
		}

		// count sets
		if (repCount >= repsNumber) {
			evaluation.endOfReps = false;
			setCount++;
			repCount = 0;
			orb.resetTarget ();
			environment.setTarget ();
			particles.activate ();

			// start interval
			if (setCount < setNumber) {
				intervalTime = 0f;
				inInterval = true;
				evaluation.isExercising = false;
				mainCanvasTransition.transition (mainCanvasTransition.pages [2]);
			}
		}

		// finish counting and calculate score
		if (setCount >= setNumber) {
			evaluation.isExercising = false;
			updatePhase ();
			saveScore ();
		}
	}

	public float getScoreOfSet(){
		float scoreSum = 0;
		for (int i = repsNumber * (setCount - 1); i < scores.Count; i++) {
			scoreSum += scores [i];
		}
		return scoreSum / repsNumber;
	}

	float getFinalScore(){
		float scoreSum = 0;
		for (int i = 0; i < scores.Count; i++) {
			scoreSum += scores [i];
		}
		return scoreSum / scores.Count;
	}

	void saveScore(){
		// generate save data
		SavedData saveData = new SavedData ();
		saveData.exerciseId = exerciseId;
		saveData.setNumber = setNumber;
		saveData.repsNumber = repsNumber;

		suggestWeight ();
		saveData.suggestedWeight = suggestedWeight;

		// compare the score of this time and the highest score the player ever had
		float finalScore = getFinalScore ();

		if (SaveAndLoad.exists (exerciseId)) {
			SavedData prevData = SaveAndLoad.load (exerciseId);

			if (prevData.highScore > finalScore) {
				saveData.highScore = finalScore;
			} else {
				saveData.highScore = prevData.highScore;
			}

		} else {
			saveData.highScore = finalScore;
		}

		// saving data
		SaveAndLoad.save (saveData, exerciseId);
	}

	void suggestWeight(){
		// suggest weight for the next time (optional)
	}

	void showResult(){
		// update phase
		phase = 0;
		device.phase = phase;
		evaluation.phase = phase;

		// assign calculated score on UI element
		scoreOnUI.text = getFinalScore().ToString();

		// display the last page
		mainCanvasTransition.transition (mainCanvasTransition.pages[3]);
	}

	void Update () {
		if (setCount <= setNumber) {
			count ();

			if (setCount == setNumber) {
				showResult ();
			}
		}
	}
}
