using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableExerciseButton : Clickable {

	// input
	public GameObject linkedPage;
	public PageTransition root;
	public ExerciseData data;
	public ExerciseButtonHandler buttonHandler;
	public Collider thisCollider;

	// output
	public Color normalColor;
	public Color pressedColor;

	// internal use
	private Image img;

	public void init(){
		base.OnEnable ();
		img = GetComponent<Image> ();
		img.color = normalColor;
	}

	public void deactivate(){
		base.OnEnable ();
		img = GetComponent<Image> ();
		img.color = pressedColor;
		this.enabled = false;
	}

	public override void pointed (bool _selected, bool _scrollUp, bool _scrollDown) {
		base.pointed (_selected, _scrollUp, _scrollDown);

		if (_selected) {
			if (!pressed) {
				img.color = pressedColor;

				// pass selected exercise data
				if (data != null) {
					buttonHandler.AssignSelectedParameters (data);
				}

				pressed = true;
				root.transition (linkedPage);

			} else {
				img.color = normalColor;
				pressed = false;
			}
		}
	}

	// deactivate collider when this is out of scroll view
	/*
	void OnCollisionrStay(Collider other){
		if(other.CompareTag("scrollable") && thisCollider.enabled == false){
			thisCollider.enabled = true;
		}
	}

	void OnCollisionExit(Collider other){
		if(other.CompareTag("scrollable") && thisCollider.enabled == true){
			thisCollider.enabled = false;
		}
	}
	*/
}
