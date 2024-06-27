using UnityEngine;
using Unity.Netcode;

namespace UU.MILDRED.Character
{
    public class NetworkLauncher : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}
