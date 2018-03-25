using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTextureSequence : MonoBehaviour {

	private Renderer rend;
	public float fps = 30f;
	public Texture[] textures;
	public string textureName;

	private float 	lenPerFrame;
	private float 	lastPhase = 0f;
	private int 	texId = 0;

	void Start(){
		rend = GetComponent<Renderer> ();
		lenPerFrame = 1f / fps;
		rend.material.SetTexture(textureName, textures [texId]);
	}

	// play image sequence
	void Update () {
		float currentPhase = Time.fixedTime % lenPerFrame;

		if (currentPhase < lastPhase) {
			texId++;
			texId = texId % textures.Length;
			rend.material.SetTexture(textureName, textures [texId]);
		}

		lastPhase = currentPhase;
	}
}
