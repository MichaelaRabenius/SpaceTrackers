using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkGUI : NetworkLobbyManager {

	private NetworkLobbyManager manager;

	// Menu Main
	private GameObject menuMain;
	private Button buttonStartServer;
	private Button buttonJoinServer;

	// Menu Server Started
	private GameObject menuServerStarted;

	// Menu Client Connecting
	private GameObject menuClientConnecting;

	// Menu Client Connected
	private GameObject menuClientConnected;

	void Awake() {
		manager = this;
	}

	// Use this for initialization
	void Start () {
		// Menu Main
		menuMain = GameObject.Find("MenuMain");
		buttonStartServer = GameObject.Find ("ButtonStartServer").GetComponent<Button> ();
		buttonJoinServer = GameObject.Find ("ButtonJoinServer").GetComponent<Button> ();

		buttonStartServer.onClick.AddListener (NetworkStartServer);
		buttonJoinServer.onClick.AddListener (NetworkJoinServer);

		// Menu Server Started
		menuServerStarted = GameObject.Find("MenuServerStarted");

		// Menu Client Connecting
		menuClientConnecting = GameObject.Find("MenuClientConnecting");

		// Menu Client Connected
		menuClientConnected = GameObject.Find("MenuClientConnected");

		// Show main menu
		showMenu(menuMain);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Update");
	}

	void OnGUI () {
		
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		NetworkJoinedServer ();
	}

	void showMenu(GameObject menu) {
		menuMain.SetActive (false);
		menuServerStarted.SetActive (false);
		menuClientConnecting.SetActive (false);
		menuClientConnected.SetActive (false);

		menu.SetActive (true);
	}

	void NetworkStartServer ()
	{
		manager.StartServer ();
		Debug.Log ("Server started");
		showMenu(menuServerStarted);
	}

	void NetworkJoinServer ()
	{
		manager.StartClient ();
		Debug.Log ("Client connecting");
		showMenu(menuClientConnecting);
	}

	void NetworkJoinedServer ()
	{
		Debug.Log ("Client connected");
		showMenu(menuClientConnected);
	}
}
