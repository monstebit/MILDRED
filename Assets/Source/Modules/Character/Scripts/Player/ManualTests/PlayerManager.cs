using System;
using Source.Modules.Core;
using Unity.Netcode;

namespace Source.Modules.Character.Scripts.Player.ManualTests
{
    public class PlayerManager : CoreNetworkSingleton<PlayerManager>
    {
        private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

        public int PlayersInGame
        {
            get
            {
                return playersInGame.Value;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                if (IsServer)
                {
                    CoreLogger.Instance.LogInfo($"Player {id} connected");
                    playersInGame.Value++;
                }
            };
        
            NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                if (IsServer)
                {
                    CoreLogger.Instance.LogInfo($"Player {id} disconnected");
                    playersInGame.Value--;
                }
            };
        }
    }
}