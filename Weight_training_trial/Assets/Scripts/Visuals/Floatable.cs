using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatable : MonoBehaviour {

	public bool 	forceEffect = false;
	public Renderer rend;
	public int 		id;
	public bool 	isActive = true;

	private Rigidbody rbd;

	// Use this for initialization
	void Start () {
		rbd = GetComponent<Rigidbody> ();
		rbd.drag = 0.5f;
		isActive = true;
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("orb")){
			forceEffect = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag("orb")){
			forceEffect = false;
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag ("controller")) {
			//Color color = rend.material.GetColor ("_Color");
			isActive = false;
			//Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void enableForce(Vector3 _force){
		rbd.AddForce (Vector3.zero);
		rbd.AddForce (_force);
	}

	public void disableForce(){
		rbd.AddForce (Vector3.zero);
	}
}
