﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class cameraRotation : NetworkBehaviour  
{
	#region [Private fields]
	private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);

	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix =  Quaternion.identity;

	private Quaternion referanceRotation = Quaternion.identity;
	private Quaternion rotateDown = Quaternion.identity;
	private Quaternion gyroCal = Quaternion.identity;

	private bool ButtonClicked = false;
	private bool showButton = true;
	public Quaternion currentGyroRot = Quaternion.identity;

	private Camera sceneCamera;
	#endregion

	#region [Unity events]

	protected void Start () 
	{
		sceneCamera = Camera.main;
		AttachGyro();

		//Create the quaternion to pitch 90 degrees, (x-axis)
		rotateDown [0] = Mathf.Sqrt (0.5f);
		rotateDown [3] = Mathf.Sqrt (0.5f);
	}

	protected void Update() 
	{	
		if (isServer)
			return;
		
		if (!gyroEnabled)
			return;

		//Calibrate gyro with button
		if (ButtonClicked) {
				gyroCal = sceneCamera.transform.rotation;
				showButton = false;
				ButtonClicked = false;
		}
		

		//Calculate the rotation with the help of gyro
		Quaternion tempQuat = cameraBase * (ConvertRotation (referanceRotation * Input.gyro.attitude) * GetRotFix ());

		//Create the quaternion to only allow yaw (y-axis)
		tempQuat = allowYaw(tempQuat);

		//Multiply the quaternions to apply the rotations
		tempQuat = calibrateGyro (tempQuat, gyroCal);

		//Rotate 90 degrees down
		tempQuat *= rotateDown;
		print ("rotationen" + rotateDown);
		print ("tempquat " + tempQuat.eulerAngles);
		//Apply the transformation to the camera
		sceneCamera.transform.rotation = Quaternion.Slerp(sceneCamera.transform.rotation, tempQuat, lowPassFilterFactor);

		//Public variable to be used by another script
		currentGyroRot = allowYaw(sceneCamera.transform.rotation);
	}		

	#endregion
	private GUIStyle mStyle = new GUIStyle();
	protected void OnGUI(){
		mStyle.fontSize = 50;
		mStyle.normal.textColor = Color.red;
		GUILayout.Label ("        ");
		GUILayout.Label("GyroRot" + this.sceneCamera.transform.rotation.eulerAngles, mStyle);

		if (showButton && isLocalPlayer) {
			if (GUI.Button (new Rect (500, 500, 200, 100), "Calibrate Gyro")) {
				ButtonClicked = true;
			}
		}

	}
	#region [Public methods]

	/// <summary>
	/// Attaches gyro controller to the transform.
	/// </summary>
	private void AttachGyro()
	{	
		//Enables gyro
		Input.gyro.enabled = !Input.gyro.enabled;
	
		gyroEnabled = true;
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}

	/// <summary>
	/// Detaches gyro controller from the transform
	/// </summary>
	private void DetachGyro()
	{
		gyroEnabled = false;
	}

	#endregion

	#region [Private methods]

	/// <summary>
	/// Update the gyro calibration.
	/// </summary>
	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}

	/// <summary>
	/// Update the camera base rotation.
	/// </summary>
	/// <param name='onlyHorizontal'>
	/// Only y rotation.
	/// </param>
	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = sceneCamera.transform.rotation;
		}
	}

	/// <summary>
	/// Converts the rotation from right handed to left handed.
	/// </summary>
	/// <returns>
	/// The result rotation.
	/// </returns>
	/// <param name='q'>
	/// The rotation to convert.
	/// </param>
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}

	/// <summary>
	/// Gets the rot fix for different orientations.
	/// </summary>
	/// <returns>
	/// The rot fix.
	/// </returns>
	private Quaternion GetRotFix()
	{
		return Quaternion.identity;
	}

	/// <summary>
	/// Recalculates reference system.
	/// </summary>
	private void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}

	/// <summary>
	/// Recalculates reference rotation.
	/// </summary>
	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}

	#endregion

	//Removes the z and x rotation
	private Quaternion allowYaw(Quaternion inputRot){

		inputRot[0] = 0;
		inputRot [2] = 0;
		float mag = Mathf.Sqrt (inputRot [3] * inputRot [3] + inputRot [1] * inputRot [1]);
		inputRot [1] /= mag;
		inputRot [3] /= mag;

		return inputRot;
	}

	//Calibrate gyro so that device points towards center of game
	private Quaternion calibrateGyro(Quaternion inputRot, Quaternion gyroCal){
		Quaternion temp = Quaternion.Inverse(allowYaw (gyroCal));
		inputRot *= temp;
		return inputRot;
	}
}