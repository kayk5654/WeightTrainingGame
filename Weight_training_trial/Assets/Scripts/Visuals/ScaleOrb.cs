﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOrb : MonoBehaviour {

	// input
	public Evaluation 		evaluation;
	public ExercisePhase 	exPhase;
	public float 			step = 0.01f;
	public float 			scaleRatio = 1.5f;
	public SphereCollider 	col;
	public ParticleSystem 	ps;
	public ParticleSystem 	lightVolume;
	public Transform 		trackingTarget;
	public Transform[] 		controllers;
	public Renderer 		rend;

	// internal use
	private Vector3 	targetScale;
	private Vector3 	beginScale;
	private Vector3 	initialScale;
	private float 		phase;
	private bool 		resetting = false;
	private bool 		activated = false;
	private bool 		scaling = false;
	private bool 		positionFixed = false;
	private float 		baseAlpha = 0.206f;
	private float 		alphaMul = 2f;
	private float 		alphaPhase = 0f;
	private bool 		alphaInitialized = false;

	void Start () {
		targetScale = transform.localScale;
		initialScale = transform.localScale;
		//initTracking ();

	}
	
	void Update () {

		if (exPhase.phase == 2 && !activated){ // if the phase of the contents is 2, activate orb
			activate ();
		}

		if (exPhase.phase != 2) {
			deactivate ();
		}

		if (!activated) { // the rest of this function is applied if the orb is activated
			return;
		}

		// set alpha level
		if (!alphaInitialized) {
			fadeInAlpha ();
		} else {
			adjustAlpha ();
		}

		// let orb tracks position and angle of the head
		if (!positionFixed) {
			tracking ();
		}

		// reset size of orb
		if (resetting || OVRInput.GetDown (OVRInput.RawButton.LThumbstickRight)) {  // for debugging purpose
			// call vfx for the end of a set
			reset ();
			particleActivate ();
		} else {
			particleDeactivate ();
		}

		// scale orb
		if (scaling) {
			scale ();
		}

		if (ps.isPlaying) {
			particleUpdate ();
		}
	}

	void initTracking(){
		transform.position = trackingTarget.position;
	}

	// fade the orb in on the beginnning
	void fadeInAlpha(){
		alphaPhase += 0.05f;

		rend.material.SetFloat("_BaseAlpha", alphaPhase * baseAlpha * 0.5f);
		rend.material.SetFloat ("_AlphaMul", alphaPhase * alphaMul * 0.5f);

		if (alphaPhase > 1f){
			alphaPhase = 1f;
			alphaInitialized = true;
		}
	}

	// adjust the level of alpha based on the target period of the next rep
	void adjustAlpha(){
		if (evaluation.isExercising) {
			float timeDiffFactor = (10f - Mathf.Abs (evaluation.nextPeakOfReps - Time.fixedTime)) * 0.1f;
			float adjustedAlpha = Mathf.MoveTowards (alphaPhase, timeDiffFactor, 0.05f);
			rend.material.SetFloat ("_BaseAlpha", adjustedAlpha * baseAlpha);
			rend.material.SetFloat ("_AlphaMul", adjustedAlpha * alphaMul);
			alphaPhase = adjustedAlpha;

		} else {
			float adjustedAlpha = Mathf.MoveTowards (alphaPhase, 0.5f, 0.05f);
			rend.material.SetFloat("_BaseAlpha", alphaPhase * baseAlpha);
			rend.material.SetFloat ("_AlphaMul", alphaPhase * alphaMul);
			alphaPhase = adjustedAlpha;
		}
	}

	// fix the position of the orb
	public void fixPosition(){
		transform.position = trackingTarget.position;
		positionFixed = true;
	}

	// let orb track the head
	void tracking(){
		float step = Time.deltaTime * (Vector3.Distance(transform.position, trackingTarget.position) * 0.2f);
		transform.position = Vector3.MoveTowards(transform.position, trackingTarget.position, step);
	}

	// set target scale of the orb
	public void setTargetScale(float _scoreOfRep = 0f){
		targetScale = transform.localScale * (scaleRatio + _scoreOfRep);
		beginScale = transform.localScale;

		phase = 0f;
		scaling = true;
	}

	// scale orb
	public void scale(){
		
		phase += step;


		float modifiedPhase = Mathf.SmoothStep(0f, 1f, phase);
		transform.localScale = Vector3.Lerp (beginScale, targetScale, modifiedPhase);

		if (phase > 1f) {
			phase = 1f;
		}

		if (transform.localScale.x > targetScale.x) {
			transform.localScale = targetScale;
			scaling = false;
		}
	}

	public void resetTarget(){
		targetScale = initialScale;
		beginScale = transform.localScale;

		phase = 0f;
		resetting = true;
	}

	// reset scale of orb
	public void reset(){
		transform.localScale = initialScale;

		phase += step*2f;


		float modifiedPhase = Mathf.SmoothStep(0f, 1f, phase);
		transform.localScale = Vector3.Lerp (beginScale, targetScale, modifiedPhase);

		if (phase > 1f) {
			phase = 1f;
		}
		if (transform.localScale.x < targetScale.x) {
			transform.localScale = targetScale;
			resetting = false;
		}
	}

	public void activate(){
		this.GetComponent<Renderer> ().enabled = true;
		rend.material.SetFloat("_BaseAlpha", 0f);
		rend.material.SetFloat ("_AlphaMul", 0f);
		lightVolume.Play ();
		activated = true;
	}

	public void deactivate(){
		this.GetComponent<Renderer> ().enabled = false;
		lightVolume.Stop ();
		activated = false;
	}

	void particleActivate(){
		ps.Play ();
	}

	void particleDeactivate(){
		ps.Stop ();
	}

	void particleUpdate(){
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
		ps.GetParticles (particles);

		for (int i = 0; i < ps.particleCount; i++) {
			// set force for particles
			particles[i].velocity = Vector3.zero;
			float dist1 = Vector3.Distance (particles [i].position, controllers [0].position);
			float dist2 = Vector3.Distance (particles [i].position, controllers [1].position);
			Vector3 force;
			if (dist1 < dist2){
				force = controllers[0].position - particles[i].position;
			} else {
				force = controllers[1].position - particles[i].position;
			}
			//particles [i].velocity = force;
			force.Scale(new Vector3(0.1f, 0.1f, 0.1f));
			particles [i].position += force;
		}

		ps.SetParticles (particles, particles.Length);
	}

	void debug(){
		evaluation.peakOfReps = OVRInput.GetDown (OVRInput.RawButton.X);

	}
}
