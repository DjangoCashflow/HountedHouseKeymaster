using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class AppConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_Text connectionStatusText;

    void Start()
    {
        connectionStatusText.text = "Connecting to server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        connectionStatusText.text = "Connected to Master";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        connectionStatusText.text = "Joined Lobby";
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        connectionStatusText.text = "Joined Room";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionStatusText.text = $"Disconnected: {cause}";
    }
}
