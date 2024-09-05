using System;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerClientLauncher : MonoBehaviour
    {
        public static PlayerClientLauncher Instance;
        
        [SerializeField] private bool _startGameAsClient;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            StartGameAsClient();
        }

        private void StartGameAsClient()
        {
            if (_startGameAsClient)
            {
                _startGameAsClient = false;
                //  WE MUST FIRST SHUT DOWN, BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN 
                NetworkManager.Singleton.Shutdown();

                //  WE MUST RESTART, AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}