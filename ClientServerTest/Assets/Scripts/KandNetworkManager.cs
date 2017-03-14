using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KandNetworkManager : NetworkBehaviour {

	public int port = 7777;
	private string serverIp;

	// START Server part
	public void HostGame() {
		bool useNat = !Network.HavePublicAddress ();

		// Start server
		Network.InitializeServer(8, port, useNat);

		Debug.Log ("Server has been initialized");
	}

	public void ShutdownServer() {
		// Shutdown server
		Network.Disconnect();

		Debug.Log ("Server has been shutdown");
	}
	// END Server part

	// START Client part
	public void ConnectToServer(string sIP) {
		serverIp = sIP;

		Network.Connect (serverIp, port);

		Debug.Log ("Connecting to: " + serverIp);
	}

	public void DisconnectFromServer() {
		Network.CloseConnection (Network.connections [0], false);

		Debug.Log ("Disconnecting from: " + serverIp);
	}

	public void OnConnectedToServer() {
		Debug.Log ("Connected to: " + serverIp);
	}
	// END Client server part

	// START Getters
	public string GetHostingIP() {
		return Network.player.ipAddress;
	}

	public int GetConnectionsCount() {
		return Network.connections.Length;
	}

	public string GetServerIp() {
		return serverIp;
	}
	// END Getters

}
