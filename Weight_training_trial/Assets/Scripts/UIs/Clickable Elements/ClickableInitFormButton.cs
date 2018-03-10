using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableInitFormButton : Clickable {

	// input
	public Evaluation evaluation;
	public FormInitProcess initProcess;
	public PageTransition root;

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
			evaluation.inInit = !evaluation.inInit;

			if (!pressed) {
				img.color = pressedColor;

				pressed = true;

			} else {
				img.color = normalColor;
				pressed = false;
			}
			/*
			if (initProcess.initPhase == 2) {
				this.enabled = false;
			}
			*/
		}
	}
}
