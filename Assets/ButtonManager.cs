using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviourPunCallbacks
{
    public Button[] doorButtons = new Button[15];
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private bool[] isCubeEnabled = new bool[15];
    private float reenableTime = 2.0f; // Default to Easy difficulty

    void Start()
    {
        for (int i = 0; i < doorButtons.Length; i++)
        {
            int index = i; // Local copy for closure
            doorButtons[i].interactable = false;
            doorButtons[i].onClick.AddListener(() => OnDoorButtonClicked(index));
        }

        easyButton.onClick.AddListener(() => SetDifficulty(2.0f, easyButton));
        mediumButton.onClick.AddListener(() => SetDifficulty(5.0f, mediumButton));
        hardButton.onClick.AddListener(() => SetDifficulty(8.0f, hardButton));

        // Set Easy as the default difficulty
        SetDifficulty(2.0f, easyButton);
    }

    public override void OnJoinedRoom()
    {
        for (int i = 0; i < doorButtons.Length; i++)
        {
            doorButtons[i].interactable = true;
        }
    }

    void OnDoorButtonClicked(int index)
    {
        if (!isCubeEnabled[index])
        {
            StartCoroutine(EnableCubeTemporarily(index));
        }
    }

    IEnumerator EnableCubeTemporarily(int index)
    {
        isCubeEnabled[index] = true;
        doorButtons[index].interactable = false;
        SendCubeState(index, true);
        yield return new WaitForSeconds(3); // Door closes after 3 seconds
        SendCubeState(index, false);
        yield return new WaitForSeconds(reenableTime); // Button re-enables based on difficulty
        isCubeEnabled[index] = false;
        doorButtons[index].interactable = true;
    }

    void SendCubeState(int index, bool state)
    {
        byte eventCode = state ? (byte)(index + 1) : (byte)(index + 100);
        PhotonNetwork.RaiseEvent(eventCode, null, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    void SetDifficulty(float time, Button selectedButton)
    {
        reenableTime = time;

        // Update button states to reflect selected difficulty
        easyButton.interactable = true;
        mediumButton.interactable = true;
        hardButton.interactable = true;

        selectedButton.interactable = false;
    }
}
