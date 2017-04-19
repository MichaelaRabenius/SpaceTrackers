using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerLobbyHook : Prototype.NetworkLobby.LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer){

		if (lobbyPlayer == null)
			return;

		Prototype.NetworkLobby.LobbyPlayer lp = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer> ();
	
		if (lp != null) {
			gameManager.AddPlayer (gamePlayer, lp.slot, lp.nameInput.text);
		}
	}

}
