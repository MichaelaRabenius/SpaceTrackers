using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkPlayerMovement : NetworkBehaviour {


	//Tracking + Camera
	private GameObject ARCamera;
	private Vector3 ARCameraPos;
	private Vector3 sceneCameraPos;
	private Vector3 scaledCameraPos;

	private const float trackingSpeed = 15f;
	private const float cameraHeight = 300f;
	private const float interpolateSpeed = 3f;

//	private bool isTracking = false;
	private Camera sceneMainCamera;
	//Tracking + Camera END

	string Local;

	// Use this for initialization
	void Start () {


		//Tracking + Camera
		sceneMainCamera = Camera.main;
		GameObject playerCube = GameObject.Find ("playerCube");
		if (!isServer) {

			ARCamera = GameObject.Find ("ARCamera");
		} else {
			print("IM HOST");
			transform.position = new Vector3 (0, cameraHeight, 0);
			playerCube.GetComponent<Renderer> ().material.color = Color.green;
			//playerCube.activeSelf = false;
			return;
		}
		//Tracking + Camera END

	}
	// Update is called once per frame
	//[ClientCallback]
	void Update () {
		
		if (this.isLocalPlayer)
			Local = "true";
		else
			Local = "false";

		GameObject playerCube = GameObject.Find ("playerCube");
		float cubeHeight = 10.0f - cameraHeight;

		//Tracking + Camera 
		if (!isServer && this.isLocalPlayer) {
			print ("IM LOCAL " + SystemInfo.deviceName);

			//Get the tracking state from the ImageTarget component
//			isTracking = GameObject.Find ("ImageTarget").GetComponent<trackingState> ().isTracking;

			//Get position
			ARCameraPos = ARCamera.transform.position;

//			if (isTracking) {

				//Scale the camera pos
				scaledCameraPos = new Vector3 (ARCameraPos.x * trackingSpeed, cameraHeight, ARCameraPos.z * trackingSpeed);

				sceneCameraPos = transform.position;
				sceneCameraPos.Set (sceneCameraPos.x, cameraHeight, sceneCameraPos.z);
				transform.position = Vector3.Lerp (sceneCameraPos, scaledCameraPos, Time.deltaTime * interpolateSpeed);
				sceneCameraPos = transform.position;
				sceneMainCamera.transform.position = sceneCameraPos;//new Vector3 (sceneCameraPos.x, cameraHeight, sceneCameraPos.z);
				playerCube.transform.localPosition.Set(playerCube.transform.localPosition.x, cubeHeight, playerCube.transform.localPosition.z);

//			}
		} else {
//			print ("IM HOST " + SystemInfo.deviceName);
		}
	}
	void OnGUI(){
		
		GUILayout.Label (Local);
	
	}

}
