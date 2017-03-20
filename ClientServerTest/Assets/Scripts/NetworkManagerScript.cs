using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerScript : NetworkLobbyManager {

	public int GetConnectionsCount() {
		return Network.connections.Length;
	}

	public string GetServerIP() {
		return Network.player.ipAddress;
	}

	public int GetServerPort() {
		return Network.player.port;
	}

}
