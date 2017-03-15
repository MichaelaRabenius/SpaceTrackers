using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour{
	/*VARIABLES*/
	private float cameraHeight = 300f;
	private float CameraSpeed = 15f;
	private bool isTracking = false;


	//CameraMovement/holder of camera variables
	public GameObject camMovement;
	private Vector3 cameraPos;

	//GameCamera variables
	public Camera sceneCamera;
	private Quaternion cameraRot;

	//VRCamera variables
	private Camera VRCamera;
	private Vector3 VRCameraPos;
	private Vector3 VRCameraPrevPos;
	private Vector3 newCameraPos;
	private Quaternion VRCameraPrevRot;
	private Quaternion VRCameraRot;



	// Use this for initialization
	void Start () {
		//Set camera pos/rot first time
		VRCamera = Camera.main; 
		VRCameraPrevPos = VRCamera.transform.position;
		VRCameraPrevRot = VRCamera.transform.rotation;
	

		//Disable the VRCamera to show the sceneCamera feed
		VRCamera.enabled = false;
		sceneCamera.enabled = true;

	}

	// Update is called once per frame
	void Update () {
		//Get the tracking state from the ImageTarget component
		isTracking = GameObject.Find("ImageTarget").GetComponent<trackingState>().isTracking;

		/*******CAMERA MOVEMENT*******/
		//Get VRCamera position
		VRCameraPos = VRCamera.transform.position;
		//Get VRCamera Rotation
		VRCameraRot = VRCamera.transform.rotation;
		print ("sceneCam" + sceneCamera.transform.localRotation.eulerAngles);


		if (!isTracking) {
			VRCameraRot = VRCameraPrevRot;

		} else {
			/*******POSITION*******/
			//Checks if the distance moved is bigger than a value to stop jittering
			//if (checkDistance (VRCameraPrevPos, VRCameraPos, 0f)) {//KANSKE INTE BEHÖVS

			//Move with the CameraSpeed and set height
			newCameraPos = new Vector3 (VRCameraPos.x * CameraSpeed, cameraHeight, VRCameraPos.z * -CameraSpeed);
			//Interpolate between old and new position
			cameraPos = camMovement.transform.position;
			cameraPos.Set (cameraPos.x, cameraHeight, cameraPos.z);
			camMovement.transform.position = Vector3.Lerp (cameraPos, newCameraPos, Time.deltaTime * 3f);
			//}
			//Update previous position
			VRCameraPrevPos = VRCameraPos;

		
		}
	}

	/******FUNCTIONS******/
	//Function checking if the difference between two angles is greater than a value
	bool checkAngle(Vector3 prevAngle, Vector3 newAngle, float angleLimit){

		//Calculate the angle of rotation
		float deltaAngle = Mathf.Abs (prevAngle.y - newAngle.y);

		//If angle is larger than limit return true
		if (deltaAngle > angleLimit) {
			return true;
		}

		return false;
	}

	//Function checking if the difference in distance between two positions greater than a value
	bool checkDistance(Vector3 prevPos, Vector3 newPos, float distLimit){
		//Calculate distance between two positions
		float deltaDist = Mathf.Abs(Mathf.Pow((prevPos.x - newPos.x),2) + Mathf.Pow((prevPos.z - newPos.z),2));


		if ( deltaDist > distLimit) {
			return true;
		}

		return false;

	}

}
