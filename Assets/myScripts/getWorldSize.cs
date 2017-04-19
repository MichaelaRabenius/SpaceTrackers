using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getWorldSize : MonoBehaviour {
	public GameObject map;
	private Vector3 midPoint;
	private Vector3 edgePoint;
	private float distance;

	private bool showMidButton = true;
	private bool showEdgeButton = false;
	private bool getMid = false;
	private bool getEdge = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (getMid) {
			midPoint = transform.position;
			getMid = false;
			showMidButton = false;
			showEdgeButton = true;
		}

		if (getEdge) {
			edgePoint = transform.position;
			getEdge = false;
			showEdgeButton = false;
		}

		if (!showEdgeButton && !showMidButton) {
			distance = Vector3.Distance (midPoint, edgePoint);
			map.transform.localScale = new Vector3 (distance, 0.001f, distance);
		}

	}

	private GUIStyle mStyle = new GUIStyle();
	protected void OnGUI(){
		mStyle.fontSize = 30;

		if (showMidButton) {
			if (GUI.Button (new Rect (500, 900, 200, 100), "Set mid-point")) {
				getMid = true;
			}
		}

		if (showEdgeButton) {
			if (GUI.Button (new Rect (500, 900, 200, 100), "Set edge-point")) {
				getEdge = true;
			}
		}

		if (!showEdgeButton && !showMidButton)
			GUILayout.Label ("Scale" + distance);

	}
}
