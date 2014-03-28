using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the MultiplayerManager and it
/// is the foundation for our Multiplayer system.
/// </summary>

public class MultiplayerScript : MonoBehaviour {


	// Variables Start____________________

	private string titleMessage = "MultiPlay ProtoType";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 25600;
	private bool useNAT = false;
	private string ipAddress;
	private string port;
	private int numberOfPlayers = 10;

	public string playerName;
	public string serverName;
	public string serverNameForClient;

	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;

	// These variables are used to define the main window.
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndent;
	private int topIndent;


	// These variables are used to define the server shutdown window.
	private Rect serverDisWindowRect;
	private int serverDisWindowWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent  = 10;

	// These variables are used to define the client disconnect window.
	private Rect clientDisWindowRect;
	private int clientDisWindowWidth = 300;
	private int clientDisWindowHeight = 170;
	private bool showDisconnectWindow = false;


	// Variables End______________________

	// Use this for initialization
	void Start () 
	{
		// Load the last used serverName from registry.
		// If the serverName is blank, then use "Server IP..."
		// as a default name.
		serverName = PlayerPrefs.GetString ("serverName");
		if (serverName == "")
		{
			serverName = "Server IP or Domain Name";
		}

		// Load the last used playerName from registry.
		// If the playerName is blank then use "Marasong01" as a default name.
		playerName = PlayerPrefs.GetString ("playerName");
		if (playerName == "")
		{
			playerName = "Marasong01";
		}

	} // close Start()
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			showDisconnectWindow = !showDisconnectWindow;
		}
	}

	void ConnectWindow(int windowID)
	{
		// Leave a gap from the header.
		GUILayout.Space (15);

		// When the player launches the game they have the option
		// to create a server or join a server. The variables
		// iWantToSetupAServer and iWantToConnectToAServer start as 
		// false so the player is presented with two buttons
		// "Setup my server" and "Connect to a server"

		if (iWantToSetupAServer == false && iWantToConnectToAServer == false)
		{
			if (GUILayout.Button ("Setup a server", GUILayout.Height(buttonHeight)))
			{
				iWantToSetupAServer = true;
			}

			GUILayout.Space (10);

			if (GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeight)))
			{
				iWantToConnectToAServer = true;
			}

			GUILayout.Space (10);

			if (Application.isWebPlayer == false && Application.isEditor == false)
			{
				if (GUILayout.Button("Exit Prototype", GUILayout.Height (buttonHeight)))
				{
					Application.Quit();
				}
			}
		}

		if (iWantToSetupAServer == true)
		{
			// The user can type a name for their server into 
			// the textfield.
			GUILayout.Label("Enter a name for your server");
			serverName = GUILayout.TextField(serverName);

			GUILayout.Space(5);

			// The user can type in the port number for their server
			// into textfield. We defined a default value above in the
			// variables as 25600.
			GUILayout.Label("Server Port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString ()));

			GUILayout.Space(10);

			if (GUILayout.Button("Start my own server", GUILayout.Height (30)))
			{
				// Create the server
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);

				// Save the serverName using PlayerPrefs.
				PlayerPrefs.SetString ("serverName", serverName);

				iWantToSetupAServer = false;
			}

			if (GUILayout.Button("Go Back", GUILayout.Height (30)))
			{
				iWantToSetupAServer = false;
			}
		} // close if (iWantToSetupAServer == true)

		if (iWantToConnectToAServer == true)
		{
			// The user can type their player name into the
			// textfield.
			GUILayout.Label("Enter your name");
			playerName = GUILayout.TextField(playerName);

			GUILayout.Space(5);

			// The player can type the IP address for the server
			// that they want to connect to into the textfield.
			GUILayout.Label("Type in Server IP or Domain name");
			connectToIP = GUILayout.TextField(connectToIP);

			GUILayout.Space(5);

			// The player can type the Port number for the server
			// they want to connect to into the textfield.
			GUILayout.Label("Server Port");
			connectionPort = int.Parse (GUILayout.TextField(connectionPort.ToString()));

			GUILayout.Space(5);


			// The player clicks on this button to establish a connection.
			if (GUILayout.Button("Connect", GUILayout.Height (25)))
			{
				// Ensure that the player cannot join a game with an empty name
				if (playerName == "")
				{
					playerName = "Marasong01";
				}

				// If the player has a name that isnot empty then attempt to join the server.
				if (playerName != "")
				{
					// Connect to a server with the IP address contained in 
					// connectToIP and with the port number contained
					// in connectionPort.
					Network.Connect(connectToIP, connectionPort);

					PlayerPrefs.SetString ("playerName", playerName);
				}
			}

			GUILayout.Space(5);

			if (GUILayout.Button("Go Back", GUILayout.Height (25)))
			{
				iWantToConnectToAServer = false;
			}

		} // close if (iWantToConnectToAServer == true)

	} // close ConnectWindow()

	void ServerDisconnectWindow(int WindowID)
	{
		GUILayout.Label ("Server name : " + serverName);

		// Show the number of players connected.
		GUILayout.Label ("Number of players connected : " + Network.connections.Length);

		// If there is as least one connection then show the average ping.
		if (Network.connections.Length >= 1)
		{
			GUILayout.Label("Ping : " + Network.GetAveragePing(Network.connections[0]));
		}

		// Shutdown the server if the user clicks on the Shutdown server button.
		if (GUILayout.Button("Shutdown server"))
		{
			Network.Disconnect();
		}

	} // close ServerDisconnectWindow(int WindowID)

	void ClientDisconnectWindow(int WindowID)
	{
		// Show the player the server they are connected to and the average ping of their connection.
		GUILayout.Label ("Conected to server : " + serverName);
		GUILayout.Label ("Ping : " + Network.GetAveragePing (Network.connections [0]));

		GUILayout.Space (7);

		// The player disconnects from the server when they press the Disconnect button.
		if (GUILayout.Button("Disconnect", GUILayout.Height(25)))
		{
			Network.Disconnect();
		}

		GUILayout.Space (5);

		// This button allows the player using a webplayer who has can gone fullscreen
		// to be able to return to the game. Pressing escape in fullscreen
		// doesnot help as that just exits fullscreen.
		if (GUILayout.Button("Return to Game", GUILayout.Height(25)))
		{
			showDisconnectWindow = false;
		}


	} // close ClientDisconnectWindow(int WindowID)

	void OnDisconnetedFromServer()
	{
		// If a player loses the connection or leaves the scene then the level is restarted on their computer.
		Application.LoadLevel (Application.loadedLevel);
	} // close OnDisconnetedFromServer()

	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		// When the player leaves the server, delete them across the network 
		// along with their RPCs so that other players no longer see them.
		Network.RemoveRPCs (networkPlayer);
		Network.DestroyPlayerObjects (networkPlayer);
	} // close OnPlayerDisconnected(NetworkPlayer networkPlayer)



	void OnPlayerConnected (NetworkPlayer networkPlayer)
	{
		networkView.RPC ("TellPlayerServerName", networkPlayer, serverName);
	} // close OnPlayerConnected (NetworkPlayer networkPlayer)


	void OnGUI()
	{
		// If the player is disconnected then run the ConnectWindow function.
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			// Determine the position of the window based on the width and
			// height of the screen. The window will be placed in the middle
			// of the screen.
			leftIndent = (Screen.width / 2) - (connectionWindowWidth / 2);
			topIndent  = (Screen.height/ 2) - (connectionWindowHeight/ 2);
			connectionWindowRect = new Rect (leftIndent, topIndent, connectionWindowWidth, connectionWindowHeight);
			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow, titleMessage);
		}

		// If the game is running as a server then run the ServerDisconnectWindow function.
		if (Network.peerType == NetworkPeerType.Server)
		{
			// Defining the Rect for the server's disconnect window.
			serverDisWindowRect = new Rect (serverDisWindowLeftIndent, serverDisWindowTopIndent, serverDisWindowWidth, serverDisWindowHeight);
			serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
		}

		// If the connection type is a client (a player) then show a window that allows
		// them to disconnect from the server.
		if (Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true)
		{
			clientDisWindowRect = new Rect (Screen.width/2 - clientDisWindowWidth/2,
			                                Screen.height/2 - clientDisWindowHeight/2,
			                                clientDisWindowWidth, clientDisWindowHeight);

			clientDisWindowRect = GUILayout.Window (1, clientDisWindowRect, ClientDisconnectWindow, "");

		}


	} // close OnGUI()


	// Used to tell the MultiplayerScript in connected players the serverName.
	// Otherwise players connection would not be able to see the name of the server.
	[RPC]
	void TellPlayerServerName (string servername)
	{
		serverName = servername;
	}
}
