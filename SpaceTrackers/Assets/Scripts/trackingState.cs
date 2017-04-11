using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class trackingState : MonoBehaviour, ITrackableEventHandler {

	public bool isTracking = false;

	private TrackableBehaviour mTrackableBehaviour;
	private bool showGUIText = false;
	private Rect textRect = new Rect (80, 80, 120, 60);
	private Text trackingLost;

	// Use this for initialization
	void Start () {
		mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
		if (mTrackableBehaviour) {
			mTrackableBehaviour.RegisterTrackableEventHandler (this);
		}
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus){
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED) {
			showGUIText = true;
			isTracking = true;
		} else {
			showGUIText = false;
			isTracking = false;
		}
	}

	protected void OnGUI(){
		if (!showGUIText) {
			GUI.contentColor = Color.red;
			GUI.Label (textRect, "Tracking Lost");
		}
	}
}
