using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkManager : NetworkBehaviour {

	static public List<NetworkSpacePlayer> sPlayers = new List<NetworkSpacePlayer> ();
	static public NetworkManager sInstance = null;

	protected bool _spawnEnemies = true;
	protected bool _running = true;

	void Awake(){
		sInstance = this;
	}

	// Use this for initialization
	void Start () {
		if (isServer) {
			//Spawna fiender
		}

		for (int i = 0; i < sPlayers.Count; i++) {
			sPlayers[i].Init ();
		}
	}
	
	[ServerCallback]
	void Update(){

		if (!_running || sPlayers.Count == 0)
			return;

	}

	public override void OnStartClient(){
		base.OnStartClient ();


	}

	IEnumerator ReturnToLoby(){
		_running = false;
		yield return new WaitForSeconds (3f);
		LobbyManager.s_Singleton.ServerReturnToLobby ();

	}
}
