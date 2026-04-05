using UnityEngine;
using GorillaLocomotion;
using GorillaNetworking;

namespace DeviceCheck
{
    public class DeviceCheck : MonoBehaviour
    {
        public bool OnVR;

        public bool OnPC;

        public bool OnEditor;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                MonoBehaviour.print("VR");
                OnVR = true;
                OnPC = false;
                OnEditor = false;
            }
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                MonoBehaviour.print("PC");
                OnVR = false;
                OnPC = true;
                OnEditor = false;
                PhotonNetwork.Disconnect();
                // reason for disconnecting the user from the server is that said player is using a PC VR version which is used for modding.
             }
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                MonoBehaviour.print("Editor");
                OnVR = false;
                OnPC = false;
                OnEditor = true;
            }
        }
    }
}
