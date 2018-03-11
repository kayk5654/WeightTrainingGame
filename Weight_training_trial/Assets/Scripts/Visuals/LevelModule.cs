using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModule : MonoBehaviour {

	public Renderer rend;

	private float alpha = 0f;

	protected virtual void Start(){
		rend.material.SetFloat ("_Alpha", alpha);
	}

	protected virtual void Update () {
		if (rend.enabled && rend.material.GetFloat("_Alpha") < 1f) {
			alpha += 0.01f;
			rend.material.SetFloat ("_Alpha", alpha);
		}
	}
}
