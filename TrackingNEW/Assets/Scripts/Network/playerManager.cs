using System;
using UnityEngine;

[Serializable]
public class playerManager{

	public Transform m_SpawnPoint;		//Spawn position of the player
	[HideInInspector]
	public int m_PlayerNumber;			//Specifies which player this is the manager for
	[HideInInspector]
	public GameObject m_Instance;		//A reference to the instance of the tank when it's created
	[HideInInspector]
	public string m_PlayerName;			//The player name set in the lobby
	[HideInInspector]

	public playerSetup m_Setup;
	public playerMovement m_Movement;

	public void Setup(){

		//Get references to the components
		m_Movement = m_Instance.GetComponent<playerMovement> ();
		m_Setup = m_Instance.GetComponent<playerSetup> ();

		//Set player number to be consistent
		m_Movement.m_PlayerNumber = m_PlayerNumber;

		//Setup
		m_Setup.m_PlayerName = m_PlayerName;
		m_Setup.m_PlayerNumber = m_PlayerNumber;

	}

	//Disable player control
	public void DisableControl(){
		m_Movement.enabled = false;
	}

	//Enable player control
	public void EnableControl(){
		m_Movement.enabled = true;
	}

	public string GetName(){
		return m_Setup.m_PlayerName;
	}

	public bool IsReady(){
		return m_Setup.m_isReady;
	}

	public void Reset(){
		m_Movement.setDefaults ();

	}
}
