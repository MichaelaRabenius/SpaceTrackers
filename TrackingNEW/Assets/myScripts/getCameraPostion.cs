using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getCameraPostion : MonoBehaviour {
	//Variables
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
		/*sVRCameraRot = VRCamera.transform.rotation;
		Vector3 VREulerRot = VRCameraRot.eulerAngles;

		//Checks if the rotation is bigger than a value to stop jittering
		if (checkAngle (VRCameraPrevRot.eulerAngles, VRCameraRot.eulerAngles)) {
			//Removes x and z rotation
			VRCameraRot = Quaternion.Euler (90f, VREulerRot.y, 0f );
			//Interpolates between old rotation and new rotation
			float rTime = 2f*Mathf.Abs (VRCameraPrevRot.eulerAngles.y - VRCameraRot.eulerAngles.y);
			sceneCamera.transform.rotation = Quaternion.Slerp(VRCameraPrevRot, VRCameraRot, Time.deltaTime * rTime);
			//Update previous rotation
			VRCameraPrevRot = VRCameraRot;
		}*/

		/*POSITION*/
		//Get VRCamera position
		VRCameraPos = VRCamera.transform.position;

		//Checks if the distance moved is bigger than a value to stop jittering
		if (checkDistance (VRCameraPrevPos, VRCameraPos)) {
			//Move with the CameraSpeed
			VRCameraPos.Set (VRCameraPos.x*CameraSpeed, cameraHeight, VRCameraPos.z*CameraSpeed);
			//Interpolate between old and new position
			sceneCamera.transform.position = Vector3.Lerp(VRCameraPrevPos, VRCameraPos, Time.deltaTime*5f);
			//Update previous position
			VRCameraPrevPos = VRCameraPos;
		}
	

	}

	//Function checking if the difference between two angles are greater than a value
	bool checkAngle(Vector3 prevAngle, Vector3 newAngle){
		/*Variables*/
		bool angleLarger = false;
		float angleLimit = 5f;
		//Calculate the angle of rotation
		float deltaAngle = Mathf.Abs (prevAngle.y - newAngle.y);

		//If angle is larger than limit return true
		if (deltaAngle > angleLimit && deltaAngle < 180) {
			return angleLarger = true;
		}

		return angleLarger;
	}

	//Function checking distance between two positions
	bool checkDistance(Vector3 prevPos, Vector3 newPos){
		/*Variables*/
		bool distLarger = false;
		float distLimit = 8f;
		//Calculate distance between two positions
		float deltaDist = Mathf.Abs (Mathf.Pow ((prevPos.x - newPos.y), 2) + Mathf.Pow ((prevPos.z - newPos.z), 2));

		if ( deltaDist > distLimit) {
			return distLarger = true;
		}

		return distLarger;

	}
}
