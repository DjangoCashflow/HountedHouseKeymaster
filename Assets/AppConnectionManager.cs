using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_Text connectionStatusText;
    public Button reconnectButton;

    void Start()
    {
        connectionStatusText.text = "Connecting to server...";
        PhotonNetwork.ConnectUsingSettings();
        reconnectButton.onClick.AddListener(ReconnectToServer);
        reconnectButton.interactable = false; // Initially disabled
    }

    public override void OnConnectedToMaster()
    {
        connectionStatusText.text = "Connected to Master";
        PhotonNetwork.JoinLobby();
        reconnectButton.interactable = false; // Disable button when connected
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
        reconnectButton.interactable = true; // Enable button when disconnected
    }

    void ReconnectToServer()
    {
        connectionStatusText.text = "Reconnecting to server...";
        PhotonNetwork.ConnectUsingSettings();
        reconnectButton.interactable = false; // Disable button while reconnecting
    }
}
