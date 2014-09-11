using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is ued to control the users chat window. It is only
/// available when the user has joined a room, or when they are in a 
/// multiplayer game. It is a draggable window with optional filtering.
/// </summary>
public class Chat : Photon.MonoBehaviour 
{
	// Constants
	private const int MAX_MESSAGES = 50;
	private const int MAX_MSG_LENGTH = 50;
	
	// Controls variables window height and width
	// plus scrolling control
	private float windowWidth = 350;
    private float windowHeight = 150;
	private Vector2 scrollPos = Vector2.zero;
	
	/// <summary>
	/// This is the structure that holds all of the message info 
	/// that is passed.
	/// </summary>
	private class Message
	{
		public string Name { get; set; }
		public string Msg { get; set; }
		public string TimeStamp { get; set; }
		
		public Message(string Name, string Msg, string TimeStamp)
		{
			this.Name = Name;
			this.Msg = Msg;
			this.TimeStamp = TimeStamp;
		}
	}
	
	// Contains the list of messages client has recieved.
	private List<Message> MSGS; //
	
	// used to get this clients username
	private PersistentData manager;
	
	// Handles for the chat window
	private Rect popupWinRect;
	private int popupWinID;
	
	// Main strings for input and user information
	private string username = string.Empty;
	private string msg = string.Empty;
	
	/// <summary>
	/// Initializes the message list, the username and organization of clent,
	/// and the window size.
	/// </summary>
	void Start() 
	{
		manager = GameObject.Find ("Persistence").GetComponent<PersistentData> ();
		
		popupWinID = 9999;
		MSGS = new List<Message>();

        //_baseWidth = Screen.width / 8;
        //_baseHeight = Screen.height / 40;

        popupWinRect = new Rect((Screen.width / 16) * 11, Screen.height - windowHeight - Screen.height * 4 / 40, windowWidth, windowHeight);
		username = PhotonNetwork.playerName;
	}
	
	/// <summary>
	/// Keeps track if client is in a room, max message count, scroll position, and if
	/// user is logged in.
	/// </summary>
	void Update() 
	{
		// If the client is in a room then do not let the max messages 
		// go over the specified limit by this classes constant
		while(MSGS.Count >= MAX_MESSAGES)
		{
			MSGS.RemoveAt(0);
		}
	}
	
	/// <summary>
	/// Calls for the chat window to be loaded if the user is in 
	/// a multiplayer room.
	/// </summary>
	void OnGUI()
	{
		if(PhotonNetwork.room != null)
		{
			popupWinRect = GUI.Window(popupWinID, popupWinRect, PopupWinFuncChat, "");
		}
	}
	
	/// <summary>
	/// Popups the window interface for chating with othe clients in the same room.
	/// </summary>
	void PopupWinFuncChat(int winID)
	{
		// Displays the messages
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
		for(int i = 0; i < MSGS.Count ; i++)
		{
			string output = MSGS[i].TimeStamp + " " + MSGS[i].Name.ToUpper() + ": "  + MSGS[i].Msg;
			GUILayout.Label(output);
		}
		GUILayout.EndScrollView();
		
		// Gets the client's message and sends it to all other user using 
		// the RPC method "SendMessage" below.
		GUILayout.BeginHorizontal();
		this.msg = GUILayout.TextField(this.msg, MAX_MSG_LENGTH, GUILayout.Width(275));
		if(GUILayout.Button("Send") || Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
		{
			photonView.RPC("SendMessage", PhotonTargets.All, username, this.msg, getTime());
			this.msg = string.Empty;
		}
		GUILayout.EndHorizontal();
		
		// Enables dragging of this window
		GUI.DragWindow();
	}
	
	/// <summary>
	/// Upon joining a room, initialize your username, organization, and send a notification
	/// to all other users that you have joined.
	/// </summary>
	void OnJoinedRoom()
    {
		MSGS.Clear();
		username = PhotonNetwork.playerName;
		photonView.RPC("SendMessage", PhotonTargets.All, username, "has joined the room.", getTime());
    }
	
	/// <summary>
	/// Notify this client when a player has left the room
	/// </summary>
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
		MSGS.Add(new Message(player.name, "has left the room.", getTime()));
		scrollPos = new Vector2(0, MAX_MESSAGES * 25);
	}
	
	/// <summary>
	/// Returns the current time in a nicely formatted way.
	/// </summary>
	string getTime()
	{
		return "[" + DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + "]";
	}
	
	/// <summary>
	/// Sends a message with given parameters to all other users in the room
	/// </summary>
	[RPC]
	void SendMessage(string username, string msg, string timeStamp)
	{
		MSGS.Add(new Message(username, msg, timeStamp));
		scrollPos = new Vector2(0, MAX_MESSAGES * 25);
	}
}
