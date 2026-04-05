using UnityEngine;
using System.Diagnostics;

public sealed class CheckForSkids : MonoBehaviour
{
    [SerializeField] string objectName;
    [SerializeField] GameObject objectReference;
    [SerializeField] bool closeGameIfDisabled = true;

    void Update()
    {
        GameObject target = objectReference != null
            ? objectReference
            : (!string.IsNullOrEmpty(objectName) ? GameObject.Find(objectName) : null);

        if (target == null) return;

        if (!target.activeInHierarchy && closeGameIfDisabled)
        {
         Application.Quit();

        }
    }
}