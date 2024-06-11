using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviourPunCallbacks
{
    public Button[] toggleButtons = new Button[15];
    private bool[] isCubeEnabled = new bool[15];

    void Start()
    {
        for (int i = 0; i < toggleButtons.Length; i++)
        {
            int index = i; // Local copy for closure
            toggleButtons[i].interactable = false;
            toggleButtons[i].onClick.AddListener(() => OnToggleButtonClicked(index));
        }
    }

    public override void OnJoinedRoom()
    {
        for (int i = 0; i < toggleButtons.Length; i++)
        {
            toggleButtons[i].interactable = true;
        }
    }

    void OnToggleButtonClicked(int index)
    {
        if (!isCubeEnabled[index])
        {
            StartCoroutine(EnableCubeTemporarily(index));
        }
    }

    IEnumerator EnableCubeTemporarily(int index)
    {
        isCubeEnabled[index] = true;
        toggleButtons[index].interactable = false;
        SendCubeState(index, true);
        yield return new WaitForSeconds(3);
        SendCubeState(index, false);
        isCubeEnabled[index] = false;
        toggleButtons[index].interactable = true;
    }

    void SendCubeState(int index, bool state)
    {
        byte eventCode = state ? (byte)(index + 1) : (byte)(index + 100);
        PhotonNetwork.RaiseEvent(eventCode, null, RaiseEventOptions.Default, SendOptions.SendReliable);
    }
}
