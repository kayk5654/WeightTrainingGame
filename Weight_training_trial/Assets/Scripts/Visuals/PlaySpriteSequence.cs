using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySpriteSequence : MonoBehaviour {

	public Image 	img;
	public float 	fps = 30f;
	public Sprite[] sprites;
	public Sprite	defaultSprite;
	public bool 	sequenceOn = false;

	private float 	lenPerFrame;
	private float 	lastPhase = 0f;
	private int 	Id = 0;

	// initialize
	void Start(){
		img = GetComponent<Image> ();
		initSequence ();
	}

	void initSequence(){
		lenPerFrame = 1f / fps;
	}

	// play image sequence
	void playSequence(){
		float currentPhase = Time.fixedTime % lenPerFrame;

		if (currentPhase < lastPhase) {
			Id++;
			Id = Id % (int)fps;
			img.sprite = sprites [Id];
		}

		lastPhase = currentPhase;
	}

	// Update is called once per frame
	void Update () {
		if (sequenceOn) {
			playSequence ();
		} else {
			img.sprite = defaultSprite;
		}
	}
}
