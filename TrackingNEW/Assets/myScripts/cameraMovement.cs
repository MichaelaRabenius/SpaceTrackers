using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour{
	/*VARIABLES*/
	private static float cameraHeight = 300f;
	private static float CameraSpeed = 15f;
	private bool isTracking = false;
	private Vector3 centerPoint = new Vector3 (0f, cameraHeight, 0f);
	private Vector3 directionToCenter;
	private Quaternion identityQuat = Quaternion.identity;

	//CameraMovement/holder of camera variables
	public GameObject camMovement;
	private Vector3 cameraPos;

	//GameCamera variables
	public Camera sceneCamera;
	private Quaternion startRotation;

	//VRCamera variables
	private Camera VRCamera;
	private Vector3 VRCameraPos;
	private Vector3 VRCameraPrevPos;
	private Vector3 newCameraPos;
	private Quaternion VRCameraRot;
	private bool firstTime = true;

	private Vector3 temp;
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
		//Get VRCamera position & rotation
		VRCameraPos = VRCamera.transform.position;
		VRCameraRot = VRCamera.transform.rotation;

		if (!isTracking) {
			

		} else {
			/*******POSITION*******/

			//ONÖDIGT
			//When the tracking target is found the first time, save the rotation relative the correctly rotated image
			if (firstTime) {
				startRotation = VRCamera.transform.rotation;
				firstTime = false;
			
				//camMovement.transform.localRotation = Quaternion.Euler (0, startRotation.eulerAngles.y, 0);
			}
			//HIT


			directionToCenter = centerPoint - VRCameraPos;
			directionToCenter.Normalize ();

			//Move with the CameraSpeed and set height
			newCameraPos = new Vector3 (VRCameraPos.x * CameraSpeed, cameraHeight, VRCameraPos.z * -CameraSpeed);
	
			//Interpolate between old and new position
			cameraPos = camMovement.transform.position;
			cameraPos.Set (cameraPos.x, cameraHeight, cameraPos.z);
			camMovement.transform.position = Vector3.Lerp (cameraPos, newCameraPos, Time.deltaTime * 3f);
	
			//Update previous position
			VRCameraPrevPos = VRCameraPos;



		}
	}
	private GUIStyle mStyle = new GUIStyle();
	protected void OnGUI(){
		mStyle.fontSize = 50;
		mStyle.normal.textColor = Color.red;
		GUILayout.Label ("startRot" + startRotation.eulerAngles, mStyle);
	}


	/******FUNCTIONS******/
	//Function checking if the difference in distance between two positions greater than a value
	bool checkDistance(Vector3 prevPos, Vector3 newPos, float distLimit){
		//Calculate distance between two positions
		float deltaDist = Mathf.Abs(Mathf.Pow((prevPos.x - newPos.x),2f) + Mathf.Pow((prevPos.z - newPos.z),2f));

		if ( deltaDist > distLimit) {
			return true;
		}
		return false;
	}
		

	//Function that get the index of the variable with the lowest value from an array
	int lowestIndex(float[] values){
		float lowestVal = values [0];
		float lowestIndex = 0f;

		//Loop through and changen the lowest value if a smaller one is encountered
		for (int i = 1; i < values.Length; i++) {
			if (values [i] < lowestVal) {
				lowestVal = values [i];
				lowestIndex = i;
			}
		}
		return (int)lowestIndex;
	}


}
