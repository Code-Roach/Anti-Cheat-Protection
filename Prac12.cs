using Photon.Pun;
using UnityEngine;

public sealed class Prac12 : MonoBehaviour
{
    [SerializeField] bool checkForAntiAfkKick = true;
    [SerializeField] bool disconnectIfDisabled = true;
    [SerializeField] bool checkForGuiMods = true;
    [SerializeField] bool checkForHoldableMenus = true;

    [SerializeField] string[] bannedObjectNames = { "AKM", "Menu", "CheatGUI", "Holdable", "Cheat", "DontSkidMe", "menu" };
    [SerializeField] string[] bannedTags = { "Cheat", "Holdable", "Menu", "DontSkidMe", "AKM", "menu" };
    [SerializeField] LayerMask bannedLayers;

    [SerializeField] PhotonNetworkController networkController;
    [SerializeField] Camera mainCamera;

    void Update()
    {
        if (checkForAntiAfkKick &&
            networkController != null &&
            networkController.disableAFKKick)
        {
            ForceDisconnect();
        }

        if (checkForGuiMods && mainCamera != null)
        {
            Canvas[] canvases = mainCamera.GetComponentsInChildren<Canvas>(true);
            if (canvases.Length > 0)
            {
                ForceDisconnect();
            }
        }

        if (checkForHoldableMenus)
        {
            ScanForHoldableMenus();
        }
    }

    void ScanForHoldableMenus()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

        for (int i = 0; i < allObjects.Length; i++)
        {
            GameObject obj = allObjects[i];

            if (!obj.activeInHierarchy)
                continue;

            if (!string.IsNullOrEmpty(bannedTag) && obj.CompareTag(bannedTag))
            {
                ForceDisconnect();
                return;
            }

            if ((bannedLayers.value & (1 << obj.layer)) != 0)
            {
                ForceDisconnect();
                return;
            }

            if (bannedObjectNames != null)
            {
                for (int n = 0; n < bannedObjectNames.Length; n++)
                {
                    if (!string.IsNullOrEmpty(bannedObjectNames[n]) &&
                        obj.name.Contains(bannedObjectNames[n]))
                    {
                        ForceDisconnect();
                        return;
                    }
                }
            }
        }
    }

    void OnDisable()
    {
        if (disconnectIfDisabled)
        {
            ForceDisconnect();
        }
    }

    void ForceDisconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();

        Application.Quit();
        enabled = false;
    }
}