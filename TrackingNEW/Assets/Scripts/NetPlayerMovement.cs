using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetPlayerMovement : NetworkBehaviour {

	private GameObject ARCamera;
	public Camera playerCamera;
	private Vector3 ARCameraPos;
	private Vector3 gamePlayerPos;
	private Vector3 scaledCameraPos;

	private const float trackingSpeed = 15f;
	private const float cameraHeight = 300f;
	private const float interpolateSpeed = 3f;

	private bool isTracking = false;

	// Use this for initialization
	void Start () {

		if (!isServer) {

			ARCamera = GameObject.Find ("ARCamera");
			playerCamera.enabled = true;

		} else {
			print ("Im Host");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!isServer) {
			print ("IM LOCAL" + SystemInfo.deviceName + "--" + SystemInfo.deviceModel);

			//Get the tracking state from ImageTarget
			isTracking = GameObject.Find ("ImageTarget").GetComponent<trackingState> ().isTracking;

			//Get ARCamera movement
			ARCameraPos = ARCamera.transform.position;

			if (isTracking) {

				//Scale the camera pos
				scaledCameraPos = new Vector3 (ARCameraPos.x * trackingSpeed, cameraHeight, ARCameraPos.z * trackingSpeed);

				//Get position of this object (the player)
				gamePlayerPos = transform.position;
				gamePlayerPos.Set (gamePlayerPos.x, cameraHeight, gamePlayerPos.z);

				//Update the position
				transform.position = Vector3.Lerp (gamePlayerPos, scaledCameraPos, Time.deltaTime * interpolateSpeed);
			}
		} else {
			print ("IM HOST " + SystemInfo.deviceName + "--" + SystemInfo.deviceModel);
		}
		
	}
}
