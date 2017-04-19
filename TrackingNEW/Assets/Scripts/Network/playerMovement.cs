using UnityEngine;
using UnityEngine.Networking;

public class playerMovement : NetworkBehaviour {

	public int m_PlayerNumber = 1;
	public float m_Speed = 15f;
	private const float cameraHeight = 300f;
	private const float interpolateSpeed = 3f;

	private GameObject ARCamera;
	private Vector3 ARCameraPos;
	private Vector3 gamePlayerPos;
	private Vector3 scaledCameraPos;

	private Camera sceneMainCamera;

	private bool isTracking = false;

	private void Start(){
		ARCamera = GameObject.Find ("ARCamera");
		sceneMainCamera = Camera.main;
	}
	[ClientCallback]
	private void Update(){

		if (isLocalPlayer) {
			//Get the tracking state from ImageTarget
			isTracking = GameObject.Find ("ImageTarget").GetComponent<trackingState> ().isTracking;

			//Get ARCamera movement
			ARCameraPos = ARCamera.transform.position;

			if (isTracking) {
				//Scale the camera pos
				scaledCameraPos = new Vector3 (ARCameraPos.x * m_Speed, cameraHeight, ARCameraPos.z * m_Speed);

				//Get position of this object (the player)
				gamePlayerPos = transform.position;
				gamePlayerPos.Set (gamePlayerPos.x, cameraHeight, gamePlayerPos.z);

				//Update the position
				transform.position = Vector3.Lerp (gamePlayerPos, scaledCameraPos, Time.deltaTime * interpolateSpeed);


				sceneMainCamera.transform.position = transform.position;
			}
		}	
	}




	public void setDefaults(){
		m_Speed = 15f;
	}
}
