using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class gameManager : NetworkBehaviour {

	static public gameManager s_Instance;

	static public List<playerManager> m_Players = new List<playerManager>();

	public float m_StartDelay = 3f;
	public float m_EndDelay = 3f;

	public GameObject m_PlayerPrefab;

	public Transform[] m_SpawnPoint;

	private bool GameOver = false;

	[HideInInspector]
	[SyncVar]
	public bool m_GameIsFinished = false;


	//Other
	private WaitForSeconds m_StartWait;
	private WaitForSeconds m_EndWait;



	void Awake(){
		s_Instance = this;
	}

	[ServerCallback]
	private void Start(){
		//Delay
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds (m_EndDelay);

		StartCoroutine (GameLoop());
	
	}

	static public void AddPlayer(GameObject player, int playerNum, string name){

		playerManager tmp = new playerManager ();
		tmp.m_Instance = player;
		tmp.m_PlayerNumber = playerNum;
		tmp.m_PlayerName = name;
		tmp.Setup ();

		m_Players.Add (tmp);
	}

	public void RemovePlayer(GameObject player){

		playerManager toRemove = null;
		foreach (var tmp in m_Players) {
			if (tmp.m_Instance == player) {
				toRemove = tmp;
				break;
			}
		}

		if(toRemove != null)
			m_Players.Remove(toRemove);
	}

	//Game loop
	private IEnumerator GameLoop(){

		while (m_Players.Count < 2)
			yield return null;

		//Wait so all are ready
		yield return new WaitForSeconds (2.0f);

		//Start round
		//yield return StartCoroutine(RoundStarting());

		//Once started, run the RoundPlaying
		yield return StartCoroutine(RoundPlaying());

		//When round over
		//yield return StartCoroutine(RoundEnding()); 

		if (GameOver)
			LobbyManager.s_Singleton.ServerReturnToLobby ();
		else
			StartCoroutine (GameLoop ());
	}

	private IEnumerator RoundStarting(){

		//Notify clients
		RpcRoundStarting();

		yield return m_StartWait;

	}

	[ClientRpc]
	void RpcRoundStarting(){

		//Reset & disable movement
		ResetAllPlayers();
		DisablePlayerControl ();

		StartCoroutine (ClientRoundStartingFade ());
	}

	private IEnumerator ClientRoundStartingFade(){
		float elapsedTime = 0f;
		float wait = m_StartDelay - 0.5f;

		yield return null;

		while (elapsedTime < wait) {
			
			elapsedTime += Time.deltaTime;

			if (elapsedTime / wait < 0.5f)
				ResetAllPlayers ();

			yield return null;
		}
	}

	private IEnumerator RoundPlaying(){

		//Notify that client that round is starting
		RpcRoundPlaying();

		yield return null;
	}

	[ClientRpc]
	void RpcRoundPlaying(){
		//Let players controll the game
		EnablePlayerControl ();
	}

	private IEnumerator RoundEnding(){
		//GameOver = true;

		RpcRoundEnding ();

		yield return m_EndWait;

	}

	[ClientRpc]
	private void RpcRoundEnding(){
		DisablePlayerControl ();
		StartCoroutine (ClientRoundEndingFade ());

	}

	private IEnumerator ClientRoundEndingFade(){

		float elapsedTime = 0.0f;
		float wait = m_EndDelay;
		while (elapsedTime < wait) {
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	private void ResetAllPlayers(){
		for (int i = 0; i < m_Players.Count; i++) {

		//	m_Players [i].m_SpawnPoint = m_SpawnPoint [m_Players [i].m_Setup.m_PlayerNumber];
		//	m_Players [i].Reset ();
		}	
	}

	private void EnablePlayerControl(){

		for (int i = 0; i < m_Players.Count; i++) {
			m_Players [i].EnableControl ();
		}
	}

	private void DisablePlayerControl(){
		for (int i = 0; i < m_Players.Count; i++) {
			m_Players [i].DisableControl ();
		}
	}
}
