using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModuleActivation : MonoBehaviour {

	public GameObject[] levelModules;

	// initialization
	void Start(){
		foreach (GameObject module in levelModules) {
			module.GetComponent<Renderer> ().enabled = false;
		}
	}

	// if an object is in the visualize range, make it visible
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("level module")){
			Renderer rend = other.transform.gameObject.GetComponent<Renderer>();

			rend.enabled = true;
		}
	}

	// if an object is out of the visualize range, make it invisible
	void OnTriggerExit(Collider other){
		if (other.CompareTag("level module")){
			Renderer rend = other.transform.gameObject.GetComponent<Renderer>();
			rend.enabled = false;
		}
	}

}
