using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class getCameraPostion : MonoBehaviour{
	/*VARIABLES*/
	private float cameraHeight = 200f;
	private float CameraSpeed = 15f;

	//VRCamera variables
	private Camera VRCamera;
	private Vector3 VRCameraPos;
	private Vector3 VRCameraPrevPos;
	private Quaternion VRCameraPrevRot;
	private Quaternion VRCameraRot;

	//GameCamera variables
	public Camera sceneCamera;
	private Vector3 cameraPos;
	private Quaternion cameraRot;

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
		
		/*ROTATION*/
		//Get rotation from VRCamera and convert it to euler angles
		/*VRCameraRot = VRCamera.transform.rotation;

		//Checks if the rotation is bigger than a value to stop jittering
		if (checkAngle (VRCameraPrevRot.eulerAngles, VRCameraRot.eulerAngles)) {
			//Removes x and z rotation
			Vector3 newRotation = new Vector3(90f, VRCameraRot.eulerAngles.y, 0f );
			//Interpolates between old rotation and new rotation
			cameraRot = sceneCamera.transform.rotation;
			sceneCamera.transform.rotation = Quaternion.Slerp(cameraRot, Quaternion.Euler(newRotation), Time.deltaTime*5f);
		}
		//Update previous rotation
		VRCameraPrevRot = VRCameraRot;
		*/


		/*POSITION*/
		//Get VRCamera position
		VRCameraPos = VRCamera.transform.position;


		//Checks if the distance moved is bigger than a value to stop jittering
		if (checkDistance (VRCameraPrevPos, VRCameraPos)) {
			//Move with the CameraSpeed and set height
			Vector3 newCameraPos = new Vector3(VRCameraPos.x*CameraSpeed, cameraHeight, VRCameraPos.z*CameraSpeed);
			//Interpolate between old and new position
			cameraPos = sceneCamera.transform.position;
			cameraPos.Set (cameraPos.x, cameraHeight, cameraPos.z);
			sceneCamera.transform.position = Vector3.Slerp(cameraPos, newCameraPos, Time.deltaTime*2f);
		}
			//Update previous position
			VRCameraPrevPos = VRCameraPos;

	}

	//Function checking if the difference between two angles are greater than a value
	bool checkAngle(Vector3 prevAngle, Vector3 newAngle){
		/*Variables*/
		float angleLimit = 2f;
		//Calculate the angle of rotation
		float deltaAngle = Mathf.Abs (prevAngle.y - newAngle.y);
		print (deltaAngle);

		//If angle is larger than limit return true
		if (deltaAngle > angleLimit) {
			return true;
		}

		return false;
	}

	//Function checking distance between two positions
	bool checkDistance(Vector3 prevPos, Vector3 newPos){
		/*Variables*/
		float distLimit = 0.00f;
		//Calculate distance between two positions
		float deltaDist = Mathf.Abs(Mathf.Pow((prevPos.x - newPos.x),2) + Mathf.Pow((prevPos.z - newPos.z),2));


		if ( deltaDist > distLimit) {
			return true;
		}

		return false;

	}
		
}
