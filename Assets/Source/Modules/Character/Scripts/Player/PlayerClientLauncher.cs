using Unity.Netcode;
using UnityEngine;

namespace UU.MILDRED.Character
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
        
        // private void Update()
        // {
        //     if (startGameAsClient)
        //     {
        //         startGameAsClient = false;
        //         //  WE MUST FIRST SHUT DOWN, BECOUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN 
        //         NetworkManager.Singleton.Shutdown();
        //
        //         //  WE MUST RESTART, AS A CLIENT
        //         NetworkManager.Singleton.StartClient();
        //     }
        // }

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