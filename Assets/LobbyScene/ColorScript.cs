using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().material.color = new Color (1, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			if (isClient && isLocalPlayer)
				CmdChangeColor ();
		}
	}

	[Command]
	void CmdChangeColor() {

		Debug.Log ("Updating colors on clients");
		RpcChangeColor ();
	}

	[ClientRpc]
	void RpcChangeColor() {

		Debug.Log ("Updating cube color");
		GetComponent<Renderer> ().material.color = new Color (0, 1, 0);
	}
}
