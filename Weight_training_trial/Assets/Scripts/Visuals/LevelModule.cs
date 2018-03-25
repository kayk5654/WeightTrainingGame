using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModule : MonoBehaviour {

	public Renderer rend;

	private float alpha = 0f;

	// set alpha 0
	protected virtual void OnEnable(){
		rend.material.SetFloat ("_Alpha", alpha);
	}

	// fade in alpha slightly
	protected virtual void Update () {
		if (rend.enabled && rend.material.GetFloat("_Alpha") < 1f) {
			alpha += 0.01f;
			rend.material.SetFloat ("_Alpha", alpha);
		}
	}
}
