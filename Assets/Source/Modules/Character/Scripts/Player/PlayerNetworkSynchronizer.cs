using Source.Modules.Shared;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerNetworkSynchronizer : NetworkBehaviour
    {
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        [SerializeField] private bool overlaySet = false;
        // private Vector3 _networkPositionVelocity;
        // private float _networkPositionSmoothTime = 0.1f;
        // private float _networkRotationSmoothTime = 0.1f;
        
        [SerializeField] public NetworkVariable<NetworkString> PlayerName = new NetworkVariable<NetworkString>();
        [SerializeField] public NetworkVariable<int> PlayerInGame = new NetworkVariable<int>();
        [SerializeField] public NetworkVariable<int> SyncedInt = new NetworkVariable<int>();
        [SerializeField] public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>();
        [SerializeField] public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>();
        [SerializeField] public NetworkVariable<NetworkString> ControlScheme = new NetworkVariable<NetworkString>(string.Empty, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] public NetworkVariable<float> MoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public override void OnNetworkSpawn()
        {
            // base.OnNetworkSpawn();
            if (IsServer)
            {
                PlayerName.Value = $"Player {OwnerClientId}";   
            }
            
            // Подписываемся на событие изменения значения
            SyncedInt.OnValueChanged += OnValueChanged;
        }
        
        public override void OnNetworkDespawn()
        {
            // Отписываемся от события изменения значения
            SyncedInt.OnValueChanged -= OnValueChanged;
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
            //  SYNC TRANSFORM & ROTATION
            if (IsOwner && IsClient)
            {
                UpdateClientPositionAndRotationServerRpc(
                    _playerCompositionRoot.PlayerView.transform.position,
                    _playerCompositionRoot.PlayerView.transform.rotation);
            }
            
            //  SYNC PLAYER NAME
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
            // Debug.Log($"Value changed from {previousValue} to {newValue}");
        }
        
        //  RPCs
        [ServerRpc]
        public void UpdateAnimationStateServerRpc(string stateName, bool state)
        {
            SyncAnimationStateClientRpc(stateName, state);
        }
        
        [ServerRpc]
        public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Quaternion newRotation)
        {
            // Обновляем данные на сервере
            NetworkPosition.Value = newPosition;
            NetworkRotation.Value = newRotation;
            
            // Отправляем данные всем клиентам
            UpdateClientPositionAndRotationClientRpc(newPosition, newRotation);
        }

        [ClientRpc]
        private void SyncAnimationStateClientRpc(string stateName, bool state)
        {
            _playerCompositionRoot.PlayerView.Animator.SetBool(stateName, state);
        }
        
        [ClientRpc]
        private void UpdateClientPositionAndRotationClientRpc(Vector3 newPosition, Quaternion newRotation)
        {
            // Обновляем позицию и поворот на всех клиентах
            if (IsOwner) return; // Пропускаем владельца, так как он уже обновил свои данные
            
            _playerCompositionRoot.PlayerView.transform.position = newPosition;
            _playerCompositionRoot.PlayerView.transform.rotation = newRotation;
        }
    }
}