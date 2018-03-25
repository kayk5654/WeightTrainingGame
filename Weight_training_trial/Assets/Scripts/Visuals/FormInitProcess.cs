using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormInitProcess : MonoBehaviour {

	public int 							initPhase = 0;
	public PlaySpriteSequence 			statusGraphics;
	public Text 						buttonText;
	public Text 						description;
	public string[] 					statusTexts;
	public string[] 					descriptionTexts;
	public ClickableTrainingPhaseButton button;
	public AudioSource 					capturingSound;

	void Start () {
		buttonText.text = statusTexts [0];
		description.text = descriptionTexts [0];
		statusGraphics.sequenceOn = false;
		button.enabled = false;
	}
	
	void Update () {
		if (initPhase == 0 || initPhase == 2) { // set default condition
			statusGraphics.sequenceOn = false;
			if (capturingSound.isPlaying) {
				capturingSound.Stop ();
			}
		} else { // when the system capturing the form of the exercise
			statusGraphics.sequenceOn = true;
			if (!capturingSound.isPlaying) {
				capturingSound.Play ();
			}
		}

		// update texts on the panel
		buttonText.text = statusTexts [initPhase];
		description.text = descriptionTexts [initPhase];

		if (initPhase == 2) {
			button.enabled = true;
		}
	}
}
