using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class playerSetup : NetworkBehaviour {

	[SyncVar]
	public string m_PlayerName;

	[SyncVar]
	public int m_PlayerNumber;

	[SyncVar]
	public bool m_isReady = false;

	public override void OnStartClient(){
		base.OnStartClient ();

		if (!isServer) {
			gameManager.AddPlayer (gameObject, m_PlayerNumber, m_PlayerName);
			print ("Server: Adding player. -playerSetup");
		}
	}

	[ClientCallback]
	public void Update(){

		if (!isLocalPlayer) {
			print("Not Local: Update. -playerSetup");
			return;
		}
			
	}

	[Command]
	public void CmdSetReady(){
		m_isReady = true;
	}

	public override void OnNetworkDestroy(){
		gameManager.s_Instance.RemovePlayer (gameObject);
	}
}
