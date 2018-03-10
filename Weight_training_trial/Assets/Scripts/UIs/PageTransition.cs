﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTransition : MonoBehaviour {

	// input
	public GameObject[] pages;

	void Start(){
		init ();
	}

	public void init(){
		for (int i = 0; i < pages.Length; i++) {
			if (i==0) {
				pages[i].SetActive (true);
			} else {
				pages[i].SetActive (false);
			}
		}
	}

	public void closeTemporarily(){
		foreach (GameObject page in pages) {
			if (page.activeSelf) {
				page.SetActive (false);
			}
		}
	}

	public void transition(GameObject _nextPage){
		foreach (GameObject page in pages) {
			if (page.activeSelf && page != _nextPage) {
				page.SetActive (false);
				continue;
			}
		}
		_nextPage.SetActive (true);
	}
}