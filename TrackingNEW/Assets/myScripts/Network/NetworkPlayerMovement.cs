using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerMovement : NetworkBehaviour {

	private GameObject ARCamera;
	public Camera sceneCamera;
	private Vector3 ARCameraPos;
	private Camera cam;
	private GameObject camera;
	private Vector3 sceneCameraPos;
	private Vector3 scaledCameraPos;

	private const float trackingSpeed = 15f;
	private const float cameraHeight = 300f;
	private const float interpolateSpeed = 3f;

	private bool isTracking = false;


	// Use this for initialization
	void Start () {
		
		if (!isServer) {

			ARCamera = GameObject.Find ("ARCamera");

			//sceneCamera = GameObject.Find ("gamePlayer").GetComponent<> ();
			//temp.enabled = false;
			//ARCamera.GetComponent<Camera> ().enabled = false;
			camera = GameObject.Find ("Camera");
			cam = camera.GetComponent<Camera> ();
			cam.enabled = false;
			sceneCamera.enabled = true;

		} else {
			print("IM HOST");
			transform.position = new Vector3 (0, cameraHeight, 0);
			GameObject playerCube = GameObject.Find ("playerCube");
			playerCube.GetComponent<Renderer> ().material.color = Color.green;
			//playerCube.activeSelf = false;
			return;
		}

			


	
	}

	


	// Update is called once per frame
	void Update () {

		if (!isServer) {
			print ("IM LOCAL " + SystemInfo.deviceName);

			//Get the tracking state from the ImageTarget component
			isTracking = GameObject.Find ("ImageTarget").GetComponent<trackingState> ().isTracking;

			//Get position
			ARCameraPos = ARCamera.transform.position;

			if (isTracking) {

				//Scale the camera pos
				scaledCameraPos = new Vector3 (ARCameraPos.x * trackingSpeed, cameraHeight, ARCameraPos.z * trackingSpeed);

				sceneCameraPos = transform.position;
				sceneCameraPos.Set (sceneCameraPos.x, cameraHeight, sceneCameraPos.z);
				transform.position = Vector3.Lerp (sceneCameraPos, scaledCameraPos, Time.deltaTime * interpolateSpeed);

			}
		} else {
			print ("IM HOST " + SystemInfo.deviceName);
		}
	}
		


}
