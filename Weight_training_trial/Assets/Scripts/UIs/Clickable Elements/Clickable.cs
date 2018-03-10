using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clickable : MonoBehaviour {

	public 	bool 	pressed = false;
	private Outline outline;

	protected virtual void OnEnable(){
		pressed = false;
	}

	public virtual void pointed (bool _selected, bool _scrollUp, bool _scrollDown) {
		// if this object is pointed by a controller, this function is executed

		// add highlight
		outline = this.gameObject.GetComponent<Outline> ();
		if (outline == null) {
			outline = this.gameObject.AddComponent<Outline> ();
			outline.effectColor = new Color (0.96f, 0.65f, 0f, 1f);
			outline.effectDistance = new Vector2(2,2);
		} else {
			outline.enabled = true;
		}

	}

	public void disableHighlight(){
		outline.enabled = false;
	}
}
