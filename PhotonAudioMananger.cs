using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public sealed class PhotonAudioMananger : MonoBehaviour
{
    [System.Serializable]
    public sealed class StaffItem
    {
        public string playFabItemId;
        public GameObject targetObject;
    }

    [SerializeField] float checkInterval = 5f;
    [SerializeField] List<StaffItem> staffItems = new List<StaffItem>();

    HashSet<string> ownedItems = new HashSet<string>();
    bool inventoryReady;

    void Start()
    {
        StartCoroutine(InventoryLoop());
        StartCoroutine(ValidationLoop());
    }

    IEnumerator InventoryLoop()
    {
        while (true)
        {
            inventoryReady = false;
            ownedItems.Clear();

            PlayFabClientAPI.GetUserInventory(
                new GetUserInventoryRequest(),
                r =>
                {
                    if (r.Inventory != null)
                        foreach (var i in r.Inventory)
                            if (!string.IsNullOrEmpty(i.ItemId))
                                ownedItems.Add(i.ItemId);

                    inventoryReady = true;
                },
                _ => inventoryReady = true
            );

            while (!inventoryReady) yield return null;
            yield return new WaitForSeconds(checkInterval);
        }
    }

    IEnumerator ValidationLoop()
    {
        while (true)
        {
            if (inventoryReady)
            {
                for (int i = 0; i < staffItems.Count; i++)
                {
                    var s = staffItems[i];
                    if (s.targetObject == null) continue;

                    if (s.targetObject.activeSelf && !ownedItems.Contains(s.playFabItemId))
                        s.targetObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}