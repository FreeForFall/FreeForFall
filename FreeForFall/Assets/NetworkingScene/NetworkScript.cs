using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkScript : MonoBehaviour {

	void Start () {
        Debug.Log("ok");
		connectToServer(GameObject.Find("SettingsManager").GetComponent<Settings>().OnlineMode);
	}

    private void connectToServer(bool online)
    {
        if(!online){
            PhotonNetwork.offlineMode = true;
            PhotonNetwork.CreateRoom("some name");
        }
        else{
            PhotonNetwork.ConnectUsingSettings("v0.1");
        }
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnJoinedLobby()
    {
        Debug.Log("Joining a random room. Status : " + PhotonNetwork.connectionStateDetailed.ToString());
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("There were no rooms available, creating one... Status : " + PhotonNetwork.connectionStateDetailed.ToString());
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined a room dede Status : " + PhotonNetwork.connectionStateDetailed.ToString());
        var player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
		player.GetComponent<PlayerController>().enabled = true;
		player.GetComponent<Controls>().enabled = true;
		player.transform.Find("PlayerView").GetComponent<Camera>().enabled = true;
		player.transform.Find("PlayerView").GetComponent<CameraController>().enabled = true;
    }
}
