using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class KandNetworkManager : NetworkLobbyManager {

	private KandGUI gui;
	private int numPlayers = 0;

	void Awake() {
		gui = GetComponent<KandGUI>();
	}

	public int GetNumPlayers() {
		return numPlayers;
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		gui.OnNetworkClientConnect ();
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		numPlayers++;
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect(conn);
		numPlayers--;
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerID) {
		base.OnServerAddPlayer (conn, playerID);

	}
}
