using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class KandPlayer : NetworkBehaviour {

	[SyncVar]
	public string name;

	//public void Start() {
	//	name = "Player";
	//}
    public void setPlayerName (string newName)
    {
        name = newName;
    }

    public string getPlayerName ()
    {
        return name;
    }

}
