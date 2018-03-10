using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEndParticleEffect : MonoBehaviour {

	// input
	public ExercisePhase 	exPhase;
	public Transform 		referencePosition;
	public ParticleSystem 	ps;
	public ParticleSystem 	shockwave;

	void init () {
		var duration = ps.main.duration;
		duration = 2f;
		var loop = ps.main.loop;
		loop = false;
	}

	void activate (){
		ps.Play ();
		shockwave.Play ();
		init ();
	}

	void deactivate (){
		ps.Stop ();
		shockwave.Stop ();
	}

	void updateParticles(){
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
		ps.GetParticles (particles);

		for (int i = 0; i < particles.Length; i++) {
			Vector3 pos = particles [i].position;
			float xzDist = Vector2.Distance (new Vector2(pos.x, pos.z), new Vector2(transform.position.x, transform.position.z));
			Vector3 force = new Vector3 (pos.x - transform.position.x, 0f, pos.z - transform.position.z).normalized;

			// decay force depending on the distance from the orb
			float decay = Mathf.Pow(1f / Vector3.Distance (transform.position, pos), 2) * 0.5f;
			force.Scale (new Vector3(decay, decay, decay));

			particles [i].velocity = force;
		}
	}

	void Update () {
		transform.position = new Vector3 (referencePosition.position.x, transform.position.y, referencePosition.position.z);

		if (exPhase.endOfSet || OVRInput.GetDown (OVRInput.RawButton.LThumbstickRight)) { // for debugging purpose
			activate ();
		}


		if (ps.isPlaying) {
			updateParticles ();
		}
	}
}
