using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableTrainingPhaseButton : Clickable {

	// input
	public GameObject 		linkedPage;
	public PageTransition 	root;
	public ExercisePhase 	exercisePhase;
	public enum OptionalFunctions {phase1to2, phase2start, phase2to1};
	public OptionalFunctions optionalFunctions;

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
		base.pointed (_selected, _scrollUp, _scrollDown);

		if (_selected) {
			if (!pressed) {
				img.color = pressedColor;
				soundFeedback ();
				pressed = true;

				switch (optionalFunctions) {
					case OptionalFunctions.phase1to2:
						exercisePhase.updatePhase ();
						root.transition (linkedPage);
						break;

					case OptionalFunctions.phase2start:
						exercisePhase.evaluation.initNextPeakOfReps ();
						root.transition (linkedPage);
						break;

					case OptionalFunctions.phase2to1:
						exercisePhase.backToPrevPhase ();
						root.transition (linkedPage);
						break;

					default:
						break;
				}

			} else {
				img.color = normalColor;
				pressed = false;
			}
		}
	}
}
