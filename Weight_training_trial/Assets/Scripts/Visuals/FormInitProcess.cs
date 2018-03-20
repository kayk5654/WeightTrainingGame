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

	// Use this for initialization
	void Start () {
		buttonText.text = statusTexts [0];
		description.text = descriptionTexts [0];
		statusGraphics.sequenceOn = false;
		button.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (initPhase == 0 || initPhase == 2) {
			statusGraphics.sequenceOn = false;
			if (capturingSound.isPlaying) {
				capturingSound.Stop ();
			}
		} else {
			statusGraphics.sequenceOn = true;
			if (!capturingSound.isPlaying) {
				capturingSound.Play ();
			}
		}

		buttonText.text = statusTexts [initPhase];
		description.text = descriptionTexts [initPhase];

		if (initPhase == 2) {
			button.enabled = true;
		}
	}
}
