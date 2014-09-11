using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HexNetwork
{
	#region Class Member Variables
	
	private Dictionary<string, GameObject> serverObjects;
	
	#endregion
	
	private void Start()
	{
		serverObjects = new Dictionary<string, GameObject>();
	}
	
	public void connect(string version)
	{
		if(PhotonNetwork.connectionState == ConnectionState.Disconnected)
		{
        	PhotonNetwork.ConnectUsingSettings(version);
		}
	}

    public void disconnect()
    {
        if (PhotonNetwork.room != null)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
	
	public void joinRandom()
	{
		if(PhotonNetwork.connected && roomAvailable())
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else if(PhotonNetwork.connected)
		{
			string roomName = "room" + ((int)(UnityEngine.Random.value * 10000)).ToString();
			PhotonNetwork.CreateRoom(roomName, true, true, 4);
		}
	}
	
	public int playersInRoom()
	{
		Room room = PhotonNetwork.room;
		if(room != null)
		{
			return room.playerCount;
		}
		return 0;
	}
	
	private bool roomAvailable()
	{
		if(PhotonNetwork.connected)
		{
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
			foreach(RoomInfo room in rooms)
			{
				if(room.playerCount < room.maxPlayers && room.open)
				{
					return true;
				}
			}
		}
		return false;
	}
	
	public void instantiateArenaObject(string prefab, Vector3 position, Quaternion rotation)
	{
		serverObjects.Add(prefab, PhotonNetwork.Instantiate(prefab, position, rotation, 0));
	}
	
	public bool removeLocalArenaObject(string prefab)
	{
		if(!serverObjects.ContainsKey(prefab))
			return false;
		PhotonNetwork.Destroy(serverObjects[prefab]);
		serverObjects.Remove(prefab);
		return true;
	}
	
	public bool checkAndSetUsername(string username)
	{
		if(username == string.Empty)
			return false;
		PhotonPlayer[] players = PhotonNetwork.playerList;
		foreach(PhotonPlayer player in players)
		{
			if(player.name == username)
				return false;
		}
		PhotonNetwork.playerName = username;
		return true;
	}
	
	public string setNetworkAlias(string alias)
	{
		System.Random rnd = new System.Random();
		int suffix = rnd.Next(1000, 10000);
		PhotonNetwork.playerName = alias + " " + suffix.ToString();
		return alias + " " + suffix.ToString();
	}
	
	public bool startGame()
	{
		if(PhotonNetwork.isMasterClient && playersInRoom() == 4)
		{
            System.Random rnd = new System.Random();
            int map = rnd.Next(0, 2);
            Debug.Log("MAP: " + map);
            switch (map)
            {
                case 0:
                    Application.LoadLevel("ArenaBadlands");
                    break;
                case 1:
                    Application.LoadLevel("ArenaSolaris");
                    break;
            }
			return true;
		}
		return false;
	}
	
	public void destroyAll()
	{
		if(PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.DestroyAll();
		}
	}
}
