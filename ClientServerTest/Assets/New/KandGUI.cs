using System;
using UnityEngine;
using UnityEngine.UI;

public class KandGUI : MonoBehaviour {

	private KandNetworkManager manager;
    private KandPlayer KandPlayer;

	// Menu Main
	private GameObject menuMain;
	private Button buttonStartServer;
	private Button buttonJoinServer;

	// Menu Server Started
	private GameObject menuServerStarted;
	private Text textNumConnections;

	// Menu Client Connecting
	private GameObject menuClientConnecting;

	// Menu Client Connected
	private GameObject menuClientConnected;
	private InputField inputPlayerName;
	private Button buttonChangePlayerName;
    private Text playerNameActive;

	void Awake() {
		manager = GetComponent<KandNetworkManager>();
        KandPlayer = GetComponent<KandPlayer>();
	}

	// Use this for initialization
	void Start () {
		// Menu Main
		menuMain = GameObject.Find("MenuMain");
		buttonStartServer = GameObject.Find ("ButtonStartServer").GetComponent<Button> ();
		buttonJoinServer = GameObject.Find ("ButtonJoinServer").GetComponent<Button> ();

		buttonStartServer.onClick.AddListener (NetworkServerStart);
		buttonJoinServer.onClick.AddListener (NetworkClientConnect);

		// Menu Server Started
		menuServerStarted = GameObject.Find("MenuServerStarted");
		textNumConnections = GameObject.Find ("TextNumConnections").GetComponent<Text>();

		// Menu Client Connecting
		menuClientConnecting = GameObject.Find("MenuClientConnecting");

		// Menu Client Connected
		menuClientConnected = GameObject.Find("MenuClientConnected");
		inputPlayerName = GameObject.Find ("InputPlayerName").GetComponent<InputField>();
		buttonChangePlayerName = GameObject.Find ("ButtonChangePlayerName").GetComponent<Button>();
        buttonChangePlayerName.onClick.AddListener (changePlayerNameOnClick);
        playerNameActive = GameObject.Find("PlayerName").GetComponent<Text>();
        //buttonChangePlayerName.onClick.AddListener (manager.CmdSetPlayerName());

        // Show main menu
        showMenu(menuMain);
	}

    // Update is called once per frame 
    void Update () {
		//Debug.Log("Update");
	}

	void OnGUI () {
		if (menuServerStarted.activeInHierarchy) {
			textNumConnections.text = string.Format ("Connections: {0}", manager.GetNumPlayers());
		}
	}

	void showMenu(GameObject menu) {
		menuMain.SetActive (false);
		menuServerStarted.SetActive (false);
		menuClientConnecting.SetActive (false);
		menuClientConnected.SetActive (false);

		menu.SetActive (true);
	}

	void NetworkServerStart ()
	{
		manager.StartServer ();
		Debug.Log ("Server started");
		showMenu(menuServerStarted);
        //1. if client connects, spawn a row 
	}

	void NetworkClientConnect ()
	{
		string ip = GameObject.Find ("InputServerHost").GetComponent<InputField> ().text;
		manager.networkAddress = ip;

		manager.StartClient ();
		Debug.Log ("Client connecting to " + ip);
		showMenu(menuClientConnecting);
	}

	public void OnNetworkClientConnect ()
	{
		Debug.Log ("Client connected");
		showMenu(menuClientConnected);

	}

    void changePlayerNameOnClick ()
    {
        string input = GameObject.Find("InputPlayerName").GetComponent<InputField>().text;
        //KandPlayer.name = input;
        playerNameActive.text =  string.Format ("Player name: " + input);
        //manager.OnServerAddPlayer();
    }
}