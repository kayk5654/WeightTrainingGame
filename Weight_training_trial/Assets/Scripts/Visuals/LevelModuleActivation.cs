using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModuleActivation : MonoBehaviour {

	public GameObject[] levelModules;

	void Start(){
		foreach (GameObject module in levelModules) {
			module.GetComponent<Renderer> ().enabled = false;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("level module")){
			Renderer rend = other.transform.gameObject.GetComponent<Renderer>();

			rend.enabled = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag("level module")){
			Renderer rend = other.transform.gameObject.GetComponent<Renderer>();
			rend.enabled = false;
		}
	}

}
