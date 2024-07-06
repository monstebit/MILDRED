using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerClientLauncher : MonoBehaviour
    {
        [SerializeField] private bool _startGameAsClient;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            if (_startGameAsClient)
            {
                StartGameAsClient();
            }
        }
        
        private void StartGameAsClient()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("NetworkManager.Singleton is null.");
                return;
            }

            if (NetworkManager.Singleton.IsClient || 
                NetworkManager.Singleton.IsHost ||
                NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.Shutdown();
            }

            NetworkManager.Singleton.StartClient();
        }
    }
}