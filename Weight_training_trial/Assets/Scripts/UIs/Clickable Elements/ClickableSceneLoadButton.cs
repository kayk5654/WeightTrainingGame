using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickableSceneLoadButton : Clickable {

	// input
	public ControllerHandler controllerHandler;
	public string scene;
	public ExerciseManagement exmanager;
	public ExerciseParamsHandler ParamsHandler;
	public enum OptionalFunction {MenuToTraining, TrainingToMenu};
	public OptionalFunction optionalFunction;

	// output
	public Color normalColor;
	public Color pressedColor;

	// internal use
	private Image img;

	protected override void OnEnable(){
		base.OnEnable ();
		img = GetComponent<Image> ();
		img.color = normalColor;
	}

	public override void pointed (bool _selected, bool _scrollUp, bool _scrollDown) {
		// add highlight on this button
		base.pointed (_selected, _scrollUp, _scrollDown);

		if (_selected) {
			if (!pressed) {
				img.color = pressedColor;
				pressed = true;

				if (optionalFunction == OptionalFunction.MenuToTraining) {
					// set ExerciseManagement parameters of selected exercise
					ParamsHandler.AssignSelectedParameters ();

					// export temp file of selected exercise
					exmanager.ExportTempExerciseData ();

				}

				SceneManager.LoadScene (scene);

			} else {
				img.color = normalColor;
				pressed = false;
			}
		}
	}
}
