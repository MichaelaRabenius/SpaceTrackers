using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagered : MonoBehaviour {
	/*
	public static GameManagered Instance {set; get;}

	public KandNetworkManager kandNetworkManager;

	public GameObject mainMenu;

	public GameObject hostServerMenu;
	public InputField inputJoinGameServer;

	public GameObject joinGameMenu;
	public GameObject connectingMenu;
	public GameObject lobbyMenu;

	void Start () {
		Instance = this;
		DontDestroyOnLoad (gameObject);

		ShowMenu (mainMenu);
	}

	public void Update() {
		// If host server menu
		if (hostServerMenu.activeInHierarchy) {
			Text connectionsCountLabel = GameObject.FindGameObjectWithTag ("ConnectionsCount").GetComponent<Text> ();
			connectionsCountLabel.text = "Connections: " + kandNetworkManager.GetConnectionsCount ();
		}

		if (lobbyMenu.activeInHierarchy) {
			Text playerNameLabel = GameObject.FindGameObjectWithTag ("PlayerName").GetComponent<Text> ();
			playerNameLabel.text = kandNetworkManager.GetPlayerName ();
		}
	}

	public void ShowMenu(GameObject menu) {
		mainMenu.SetActive (false);
		hostServerMenu.SetActive (false);
		joinGameMenu.SetActive (false);
		connectingMenu.SetActive (false);
		lobbyMenu.SetActive (false);

		menu.SetActive (true);
	}

	// START Server part
	public void HostGame() {
		// Start server
		kandNetworkManager.HostGame ();
		ShowMenu(hostServerMenu);

		// Show server ip in menu
		Text serverLabel = GameObject.FindGameObjectWithTag ("ServerHostname").GetComponent<Text> ();
		string ip = kandNetworkManager.GetHostingIP ();
		serverLabel.text = ip;
	}
	// END Server part

	// START Client part
	public void JoinGame() {
		string serverInput = inputJoinGameServer.text;
		kandNetworkManager.ConnectToServer (serverInput);

		ShowMenu (connectingMenu);
	}

	public void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);

		if (!kandNetworkManager.isServer)
			ShowMenu(joinGameMenu);
	}

	public void OnConnectedToServer() {
		ShowMenu (lobbyMenu);
	}
	// END Client part
	*/
}
