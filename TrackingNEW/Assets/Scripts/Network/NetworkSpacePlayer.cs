using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class NetworkSpacePlayer : NetworkBehaviour {

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

	[SyncVar]
	public string playerName;

	protected bool _wasInit = false;

	void Awake(){
		NetworkManager.sPlayers.Add (this);
	}

	// Use this for initialization
	void Start () {

		ARCamera = GameObject.Find ("ARCamera");
		sceneMainCamera = Camera.main;

		if (NetworkManager.sInstance != null)
			Init ();
	}
	
	public void Init(){

		if (_wasInit)
			return;

	}

	void OnDestroy(){
		NetworkManager.sPlayers.Remove (this);
	}

	[ClientCallback]
	void Update(){

		if (!isLocalPlayer)
			return;

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
