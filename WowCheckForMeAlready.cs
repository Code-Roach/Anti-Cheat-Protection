using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public sealed class WowCheckForMeAlready : MonoBehaviourPunCallbacks
{
    int expectedMasterActor;
    readonly HashSet<int> legitMasterHistory = new HashSet<int>();

    void Start()
    {
        if (!PhotonNetwork.InRoom) return;

        expectedMasterActor = PhotonNetwork.MasterClient.ActorNumber;
        legitMasterHistory.Add(expectedMasterActor);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.ActorNumber == expectedMasterActor)
        {
            if (PhotonNetwork.MasterClient != null)
            {
                expectedMasterActor = PhotonNetwork.MasterClient.ActorNumber;
                legitMasterHistory.Add(expectedMasterActor);
            }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (legitMasterHistory.Contains(newMasterClient.ActorNumber))
            return;

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}