﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagerTwo : MonoBehaviour {

	public static GameManagerTwo Instance {set; get;}

	public NetworkManagerScript networkManager;

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
			connectionsCountLabel.text = "Connections: " + networkManager.GetConnectionsCount ();
		}

		if (lobbyMenu.activeInHierarchy) {
			//Text playerNameLabel = GameObject.FindGameObjectWithTag ("PlayerName").GetComponent<Text> ();
			//playerNameLabel.text = kandNetworkManager.GetPlayerName ();
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
		NetworkManager.singleton.networkPort = 7777;
		NetworkManager.singleton.StartServer();

		Debug.Log (NetworkManager.singleton.networkAddress);
		Debug.Log (NetworkServer.listenPort);
		Debug.Log (NetworkServer.active);

		ShowMenu(hostServerMenu);

		// Show server ip in menu
		Text serverLabel = GameObject.FindGameObjectWithTag ("ServerHostname").GetComponent<Text> ();
		string ip = networkManager.GetServerIP ();
		serverLabel.text = ip;
	}

	// END Server part

	// START Client part
	public void JoinGame() {
		string serverInput = inputJoinGameServer.text;
		//networkManager.StartClient(serverInput);
		var myClient = new NetworkClient ();
		myClient.Connect ("127.0.0.1", 7777);

		ShowMenu (connectingMenu);
	}

	public void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}

	public void OnConnectedToServer() {
		ShowMenu (lobbyMenu);
	}
	// END Client part
}