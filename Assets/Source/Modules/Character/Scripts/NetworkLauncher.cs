using TMPro;
using Unity.Netcode;
using UnityEngine;
using Source.Modules.Core;

namespace Source.Modules.Character.Scripts
{
    public class NetworkLauncher : MonoBehaviour
    {
        [SerializeField] private float fadeDuration;
        [SerializeField] private float delayDuration;
        private Coroutine logUpdateCoroutine;

        public void StartNetworkAsHost()
        {
            if (NetworkManager.Singleton.StartHost())
            {
                CoreLogger.Instance.LogInfo("Host started...");
            }
            else
            {
                CoreLogger.Instance.LogInfo("Unable to start host...");
            }
        }
        
        public void StartNetworkAsClient()
        {
            if (NetworkManager.Singleton.StartClient())
            {
                CoreLogger.Instance.LogInfo("Client started...");
            }
            else
            {
                CoreLogger.Instance.LogInfo("Unable to start client...");
            }
        }
    }
}