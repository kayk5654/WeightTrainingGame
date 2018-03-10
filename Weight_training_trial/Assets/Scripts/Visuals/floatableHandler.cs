using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatableHandler : MonoBehaviour {

	public float 		decayPower = 2f;
	public GameObject[] sampleFloatables;
	public Transform 	root;

	private List<Floatable> floatables;

	// Use this for initialization
	void Start () {
		floatables = new List<Floatable> ();

		for (int i = 0; i < 10; i++) {
			instantiateFloatables ();
		}
	}

	void applyForce(){
		for (int i = 0; i < floatables.Count; i++) {
			if (!floatables [i].forceEffect) { // if the floatable object is outside of the orb
				Vector3 pos = floatables [i].transform.position;

				Vector3 force = new Vector3 (0f - pos.x, -1f, 0f - pos.z).normalized;

				// decay magnitude of the force by the distance from origin
				float decay = Mathf.Pow(1f / Vector3.Distance (Vector3.zero, pos), decayPower) * 0.5f;
				force.Scale (new Vector3(decay, decay, decay));

				floatables [i].enableForce (force);

			} else { // if the floatable object is inside of the orb
				float height = Mathf.Sin (Time.time + i*0.1f) * 0.5f + 0.2f;
				Vector3 force = new Vector3 (0f, height, 0f);
				floatables [i].enableForce (force);
			}
		}
	}

	// Update is called once per frame
	void Update () {

		if (Random.value < 0.01f) {
			instantiateFloatables ();
		}

		remove ();

		applyForce ();
	}

	void instantiateFloatables(){

		Vector3 initPos = new Vector3 (Random.Range(-4f, 4f), Random.Range(2f, 5f), Random.Range(-4f, 4f));

		Quaternion initRot = new Quaternion (Random.Range (0f,360f), Random.Range (0f,360f), Random.Range (0f,360f), Random.Range (0f,360f));

		GameObject newObject = Instantiate (sampleFloatables [Random.Range (0, sampleFloatables.Length - 1)], initPos, initRot, root);
		newObject.SetActive (true);

		Floatable newFloatable = newObject.GetComponent<Floatable> ();
		//newFloatable.id = floatables[floatables.Count-1].id+1;

		floatables.Add (newFloatable);
	}

	void remove(){

		if (floatables.Count > 15) {
			for (int i = 0; i < floatables.Count - 15; i++) {
				floatables [i].isActive = false;
			}
		}

		for (int i = 0; i < floatables.Count; i++) {
			Floatable floatable = floatables [i];
			if (!floatables [i].isActive) {
				Destroy(floatable.gameObject);
				floatables.Remove (floatable);

			}
		}
	}
}
