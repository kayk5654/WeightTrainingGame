using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormInitProcess : MonoBehaviour {

	public int 							initPhase = 0;
	public PlaySpriteSequence 			statusGraphics;
	public Text 						textOutput;
	public string[] 					statusTexts;
	public ClickableTrainingPhaseButton button;
	public AudioSource 					capturingSound;

	// Use this for initialization
	void Start () {
		textOutput.text = statusTexts [0];
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

		textOutput.text = statusTexts [initPhase];

		if (initPhase == 2) {
			button.enabled = true;
		}
	}
}
