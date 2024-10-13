using Source.Modules.Shared;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerNetworkSynchronizer : NetworkBehaviour
    {
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;

        private NetworkVariable<int> playerInGame = new NetworkVariable<int>();
        
        [Header("Player Name")] 
        [SerializeField] private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();
        private bool overlaySet = false;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                playerName.Value = $"Player {OwnerClientId}";   
            }
        }

        public void SetOverlay()
        {
            var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = playerName.Value.ToString();
        }

        private void Update()
        {
            if (!overlaySet && !string.IsNullOrEmpty(playerName.Value))
            {
                SetOverlay();
                overlaySet = true;
            }
        }


        //ON TESTING
        [Header("Control Scheme")]
        public NetworkVariable<float> ControlScheme = 
            new NetworkVariable<float>(
                0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Position")] 
        public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(
            Vector3.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(
            Quaternion.identity,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        
        public Vector3 NetworkPositionVelocity;
        public float NetworkPositionSmoothTime = 0.1f;
        public float NetworkRotationSmoothTime = 0.1f;
        
        [Header("Locomotion")]
        public NetworkVariable<float> MoveAmount = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerCompositionRoot is null");
            }
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                if (IsServer)
                {
                    playerInGame.Value++;
                }
            };
            
            NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                if (IsServer)
                {
                    playerInGame.Value--;
                }
            };
        }

        //  только сервер может инициировать обновление состояния
        [ServerRpc]
        // public void UpdateAnimationStateServerRpc(ulong clientID, string stateName, bool state)
        public void UpdateAnimationStateServerRpc(string stateName, bool state)
        {
            //  IF THIS CHARACTER IS THE HOST/SERVER, THEN ACTIVATE THE CLIENT RPC
            if (IsServer)
            {
                SyncAnimationStateClientRpc(stateName, state);
            }
        }

        [ClientRpc]
        private void SyncAnimationStateClientRpc(string stateName, bool state)
        {
            _playerCompositionRoot.PlayerView.Animator.SetBool(stateName, state);
        }
    }
}