using Source.Modules.Shared;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerNetworkSynchronizer : NetworkBehaviour
    {
        public NetworkVariable<int> SyncedInt = new NetworkVariable<int>();
        //
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        [SerializeField] private bool overlaySet = false;
        
        [SerializeField] public NetworkVariable<float> MoveAmount = new NetworkVariable<float>(
            0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] public NetworkVariable<float> ControlScheme = new NetworkVariable<float>(
                0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        #region POSITION & ROTATION
        [SerializeField] public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>();
        [SerializeField] public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>();
        // [SerializeField] public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(
        //     Vector3.zero,
        //     NetworkVariableReadPermission.Everyone,
        //     NetworkVariableWritePermission.Owner);
        // [SerializeField] public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(
        //     Quaternion.identity,
        //     NetworkVariableReadPermission.Everyone,
        //     NetworkVariableWritePermission.Owner);
        #endregion
        
        public NetworkVariable<NetworkString> PlayerName = new NetworkVariable<NetworkString>();
        public NetworkVariable<int> PlayerInGame = new NetworkVariable<int>();
        
        public Vector3 NetworkPositionVelocity;
        public float NetworkPositionSmoothTime = 0.1f;
        public float NetworkRotationSmoothTime = 0.1f;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                PlayerName.Value = $"Player {OwnerClientId}";   
            }
            
            // Подписываемся на событие изменения значения
            SyncedInt.OnValueChanged += OnValueChanged;
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                if (IsServer)
                {
                    PlayerInGame.Value++;
                }
            };
            
            NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                if (IsServer)
                {
                    PlayerInGame.Value--;
                }
            };
        }
        
        private void Update()
        {
            if (!overlaySet && !string.IsNullOrEmpty(PlayerName.Value))
            {
                SetOverlay();
                overlaySet = true;
            }
            
            // Пример изменения значения на хосте
            if (IsServer && Input.GetKeyDown(KeyCode.Space))
            {
                ChangeValue(SyncedInt.Value + 1);
            }
        }
        
        private void SetOverlay()
        {
            var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = PlayerName.Value.ToString();
        }
        
        // Метод для изменения значения на хосте
        private void ChangeValue(int newValue)
        {
            if (IsServer)
            {
                SyncedInt.Value = newValue;
            }
        }
        
        // Метод для отображения значения на клиентах
        private void OnValueChanged(int previousValue, int newValue)
        {
            Debug.Log($"Value changed from {previousValue} to {newValue}");
        }

        public override void OnNetworkDespawn()
        {
            // Отписываемся от события изменения значения
            SyncedInt.OnValueChanged -= OnValueChanged;
        }
    }
}