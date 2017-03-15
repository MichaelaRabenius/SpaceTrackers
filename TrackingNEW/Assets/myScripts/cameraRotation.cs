using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour  
{
	#region [Private fields]
	private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	private readonly Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	private readonly Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	private readonly Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);

	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix =  Quaternion.identity;

	private Quaternion referanceRotation = Quaternion.identity;

	private Quaternion rotateDown = Quaternion.identity;

	#endregion

	#region [Unity events]

	protected void Start () 
	{
		AttachGyro();
	}

	protected void Update() 
	{
		if (!gyroEnabled)
			return;
		
		//Calculate the rotation with the help of gyro
		Quaternion tempQuat = cameraBase * (ConvertRotation (referanceRotation * Input.gyro.attitude) * GetRotFix ());

		//Create the quaternion to only allow yaw (y-axis)
		tempQuat[0] = 0;
		tempQuat [2] = 0;
		float mag = Mathf.Sqrt (tempQuat [3] * tempQuat [3] + tempQuat [1] * tempQuat [1]);
		tempQuat [1] /= mag;
		tempQuat [3] /= mag;

		//Create the quaternion to pitch 90 degrees, (x-axis)
		rotateDown [0] = Mathf.Sqrt (0.5f);
		rotateDown [3] = Mathf.Sqrt (0.5f);

		//Multiply the quaternions to apply the rotations
		tempQuat *= rotateDown;

		//Apply the transformation to the camera
		transform.rotation = Quaternion.Slerp(transform.rotation, tempQuat, lowPassFilterFactor);
	}		

	#endregion

	#region [Public methods]

	/// <summary>
	/// Attaches gyro controller to the transform.
	/// </summary>
	private void AttachGyro()
	{
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
			cameraBase = transform.rotation;
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
}
