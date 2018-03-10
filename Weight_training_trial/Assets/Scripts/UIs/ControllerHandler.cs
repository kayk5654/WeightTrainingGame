using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHandler : MonoBehaviour
{

	// input
	public Transform head;
	public GameObject interruptCanvas;
	public GameObject	mainCanvas;
	public bool interruptMenuActivated = false;
	public HapticFeedback[] controllerHaptics;

	// output
	public bool scrollUp;
	public bool scrollDown;
	public Vector2 scrollDir;
	public bool selecting = false;
	public Vector3 forwardVec;
	public Vector3 rayOrigin;
	public Transform hitpoint;

	// internal use
	private LineRenderer line;
	private GameObject lastHitUIElement;
	private GameObject lastOpenedPage;

	void Start ()
	{
		// initialize ray visualizer
		line = GetComponent<LineRenderer> ();
		Vector3[] positions = { rayOrigin, rayOrigin + forwardVec };
		line.SetPositions (positions);
	}

	// visualize ray for selection
	void visualizeRay (float raylength = 5f)
	{
		Vector3 extendedForwardVec = Vector3.Scale (forwardVec, new Vector3 (raylength, raylength, raylength));

		Vector3[] positions = { rayOrigin, (rayOrigin + extendedForwardVec) };
		line.SetPositions (positions);
	}


	void Update ()
	{
		//visualizeRay ();

		UIRaycasting ();

		pause ();
	}

	void UIRaycasting ()
	{
		// raycast for ui operation
		RaycastHit[] hits = Physics.RaycastAll (rayOrigin, forwardVec, 50f);

		for (int j = 0; j < hits.Length; j++) {
			
			if (hits [j].transform.gameObject.layer == 5) { // enable pointer visualization if the layer is ui
				//if (hits.Length > 0 && hits[0].transform.gameObject.layer == 5) { 

				// enable pointer visualization
				hitpoint.gameObject.GetComponent<MeshRenderer> ().enabled = true;
				// set pointer visualization
				hitpoint.position = hits [j].point;
				hitpoint.LookAt (head);

				// enable raycast visualization
				visualizeRay (Vector3.Distance (rayOrigin, hits [j].point));

				Clickable clickable = null;

				for (int i = j; i < hits.Length; i++) {
					if (hits [i].transform.gameObject.CompareTag ("clickable")) {
						clickable = hits [i].transform.gameObject.GetComponent<Clickable> ();
						continue;
					}

					if (hits [i].transform.gameObject.CompareTag ("scrollable")) {
						hits [i].transform.gameObject.GetComponent<ScrollRectHandler> ().pointed (scrollDir.y);
					}
				}


				if (clickable != null && clickable.enabled) {

					// disable highlight on a object pointed previously
					if (lastHitUIElement != null && lastHitUIElement != clickable.transform.gameObject) {
						lastHitUIElement.GetComponent<Clickable> ().disableHighlight ();
					}

					// set haptic feedback when player points or selects any ui elements
					if (lastHitUIElement == null || lastHitUIElement != clickable.transform.gameObject || selecting || scrollUp || scrollDown) {
						activateHapics ();
					}

					// add highlight on currently selected object
					clickable.pointed (selecting, scrollUp, scrollDown);
					lastHitUIElement = clickable.transform.gameObject;
				}
				return;
			}

		}

		// in case the ray doesn't hit the object in UI layer.z
		//disable pointer visualization
		hitpoint.gameObject.GetComponent<MeshRenderer> ().enabled = false;

		// enable ray visualization
		visualizeRay ();

		// deactivate highlight on UI elements when ray does't hit UI 
		if (lastHitUIElement != null) {
			lastHitUIElement.GetComponent<Clickable> ().disableHighlight ();
			lastHitUIElement = null;
		}
	}

	void activateHapics(){
		// set haptic feedback on controllers
		foreach(HapticFeedback haptic in controllerHaptics){
			haptic.UIHaptics ();
		}
	}

	void pause ()
	{
		// call interrupt menu of this app
		if (interruptCanvas != null) {
			interruptCanvas.SetActive (interruptMenuActivated);

			// when interrupt menu is opened, main manu is deactivated
			PageTransition mainCanvasTransition = mainCanvas.GetComponent<PageTransition> ();
			if (interruptMenuActivated) {
				foreach (GameObject page in mainCanvasTransition.pages) {
					if (page.activeSelf) {
						lastOpenedPage = page;
					}
				}
				mainCanvasTransition.closeTemporarily ();
			} else if (lastOpenedPage != null) {
				mainCanvasTransition.transition (lastOpenedPage);
			}
		}
	}
}
