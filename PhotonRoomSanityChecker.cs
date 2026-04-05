using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

internal sealed class PhotonRoomSanityChecker : MonoBehaviourPunCallbacks
{
    [SerializeField] int maxPlayersLimit = 10;
    [SerializeField] bool enforceOpen = true;
    [SerializeField] bool enforceVisible = true;
    [SerializeField] string requiredRegion = "";
    [SerializeField] string[] forbiddenProps;

    void Start()
    {
        Validate();
    }

    void Validate()
    {
        if (!PhotonNetwork.InRoom) return;

        Room r = PhotonNetwork.CurrentRoom;

        if (r.MaxPlayers > maxPlayersLimit) Leave();
        if (enforceOpen && !r.IsOpen) Leave();
        if (enforceVisible && !r.IsVisible) Leave();
        if (!string.IsNullOrEmpty(requiredRegion) && PhotonNetwork.CloudRegion != requiredRegion) Leave();

        if (forbiddenProps != null && r.CustomProperties != null)
        {
            foreach (string k in forbiddenProps)
            {
                if (r.CustomProperties.ContainsKey(k))
                {
                    Leave();
                    return;
                }
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Validate();
    }

    void Leave()
    {
        PhotonNetwork.LeaveRoom(false);
        enabled = false;
    }
}