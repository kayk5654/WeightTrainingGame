using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ClickableResetButton : Clickable {

	// input
	public GameObject linkedPage;
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
			if (!pressed) {
				img.color = pressedColor;
				pressed = true;
				reset ();
				root.transition (linkedPage);

			} else {
				img.color = normalColor;
				pressed = false;
			}
		}
	}

	void reset(){
		string filepath = Application.persistentDataPath + "saveData.json";

		if (File.Exists (filepath)) {
			SaveAndLoad.reset ();
		}
	}
}
