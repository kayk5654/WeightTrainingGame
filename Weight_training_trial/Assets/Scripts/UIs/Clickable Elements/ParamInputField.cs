using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamInputField : Clickable {

	public Text 	inputText;
	public float 	selectedValue = 0f;
	public float 	valueMin;
	public float 	valueMax;
	public float 	step;
	public enum InputType {weight, sets, reps, interval};
	public InputType inputType;

	public override void pointed (bool _selected, bool _scrollUp, bool _scrollDown){
		base.pointed (_selected, _scrollUp, _scrollDown);
		selectValue (_scrollUp, _scrollDown);
		display ();
	}

	public void init(int _exerciseId){
		// set default value for input field by save data or minimum value
		SavedData prevData = SaveAndLoad.load (_exerciseId);

		if (prevData == null) {
			selectedValue = valueMin;

		} else {
			switch (inputType) {
				case InputType.weight:
					selectedValue = prevData.suggestedWeight;
					break;

				case InputType.sets:
					selectedValue = prevData.setNumber;
					break;

				case InputType.reps:
					selectedValue = prevData.repsNumber;
					break;

				case InputType.interval:
					selectedValue = valueMin;
					break;

				default:
					selectedValue = valueMin;
					break;
			}
		}
	}

	public void display(){

		switch (inputType) {
			case InputType.weight:
				inputText.text = selectedValue.ToString("F1") + "kg";
				break;

			case InputType.sets:
				inputText.text = Mathf.RoundToInt(selectedValue).ToString() + " sets";
				break;

			case InputType.reps:
				inputText.text = Mathf.RoundToInt(selectedValue).ToString() + " reps";
				break;

			case InputType.interval:
				inputText.text = selectedValue.ToString("F1") + "mins";
				break;

			default:
				inputText.text = "--";
				break;
		}

	}

	void selectValue(bool _scrollUp, bool _scrollDown){
		if (_scrollDown && selectedValue > valueMin) {
			selectedValue -= step;

			if (selectedValue < valueMin) {
				selectedValue = valueMin;
			}

		} else if (_scrollUp && selectedValue < valueMax) {
			selectedValue += step;

			if (selectedValue > valueMax) {
				selectedValue = valueMax;
			}

		}
	}
}
