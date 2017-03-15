using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

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

	//VRCamera variables
	private Camera VRCamera;
	private Vector3 VRCameraPos;
	private Vector3 VRCameraPrevPos;
	private Vector3 newCameraPos;
	private Quaternion VRCameraRot;



	// Use this for initialization
	void Start () {
		//Set camera pos/rot first time
		VRCamera = Camera.main; 
		VRCameraPrevPos = VRCamera.transform.position;
	

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
		VRCameraRot = VRCamera.transform.rotation;

		if (!isTracking) {
			

		} else {
			/*******POSITION*******/
			//Checks if the distance moved is bigger than a value to stop jittering
			//if (checkDistance (VRCameraPrevPos, VRCameraPos, 0f)) {//KANSKE INTE BEHÖVS

			//Move with the CameraSpeed and set height

			Vector2 cameraDirection = calculateCameraDirection (VRCameraRot, VRCameraPos);
			newCameraPos = new Vector3 (cameraDirection.x * CameraSpeed, cameraHeight, cameraDirection.y * -CameraSpeed);
			//Interpolate between old and new position
			cameraPos = camMovement.transform.position;
			cameraPos.Set (cameraPos.x, cameraHeight, cameraPos.z);
			camMovement.transform.position = Vector3.Lerp (cameraPos, newCameraPos, Time.deltaTime * 3f);
			//}
			//Update previous position
			VRCameraPrevPos = VRCameraPos;

		
		}
	}
	private GUIStyle mStyle = new GUIStyle();
	protected void OnGUI(){
		mStyle.fontSize = 50;
		mStyle.normal.textColor = Color.red;
		GUILayout.Label("ROT" + VRCamera.transform.rotation.eulerAngles, mStyle);
	}


	/******FUNCTIONS******/
	//Function checking if the difference in distance between two positions greater than a value
	bool checkDistance(Vector3 prevPos, Vector3 newPos, float distLimit){
		//Calculate distance between two positions
		float deltaDist = Mathf.Abs(Mathf.Pow((prevPos.x - newPos.x),2) + Mathf.Pow((prevPos.z - newPos.z),2));


		if ( deltaDist > distLimit) {
			return true;
		}

		return false;

	}

	//Function recalculate the cameraspeed direction depening on the angle the camera is tracking the marker
	Vector2 calculateCameraDirection(Quaternion cameraRot, Vector3 cameraPos){
		//Variables
		Quaternion [] constantAngles = new Quaternion[4];
		constantAngles[0] = Quaternion.identity;
		constantAngles[1] = Quaternion.Euler (0, 90, 0);
		constantAngles[2] = Quaternion.Euler (0, 180, 0);
		constantAngles[3] = Quaternion.Euler (0, 270, 0);

		Vector2 resultDirection = new Vector2();

		//Remove x and z rotation from camera
		Quaternion tempRot = cameraRot;
		tempRot [0] = 0;
		tempRot [2] = 0;
		float mag = Mathf.Sqrt (tempRot [3] * tempRot [3] + tempRot [1] * tempRot [1]);
		tempRot [1] /= mag;
		tempRot [3] /= mag;

		//Variable to hold the difference between angles
		float [] differenceAngles = new float[4];

		//Calculate the difference between angles
		for (int i = 0; i < 4; i++) {
			differenceAngles [i] = Quaternion.Angle (tempRot, constantAngles [i]);

		}
		//Get the index of the angle with the smallest angle between the camera and the target
		int index = lowestIndex (differenceAngles);
	

		//Chose the right configuration for the calculated angle
		switch (index) {
		case 0:
			resultDirection.x = -cameraPos.z;
			resultDirection.y = cameraPos.x;
			break;
		case 1:
			resultDirection.x = -cameraPos.x;
			resultDirection.y = -cameraPos.z;
			break;
		case 2:
			resultDirection.x = cameraPos.z;
			resultDirection.y = -cameraPos.x;
			break;
		case 3:
			resultDirection.x = cameraPos.x;
			resultDirection.y = cameraPos.z;
			break;
		default:
			print ("Error getting camera direction!");
			break;
		}

		return resultDirection;

	}

	int lowestIndex(float[] values){
		float lowestVal = values [0];
		float lowestIndex = 0;

		for (int i = 1; i < values.Length; i++) {
			if (values [i] < lowestVal) {
				lowestVal = values [i];
				lowestIndex = i;
			}
		}
		return (int)lowestIndex;
	}

}
