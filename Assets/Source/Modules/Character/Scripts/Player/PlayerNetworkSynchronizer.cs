using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerNetworkSynchronizer : NetworkBehaviour
    {
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;

        [Header("Player Name")] 
        public NetworkVariable<FixedString128Bytes> characterName = 
            new NetworkVariable<FixedString128Bytes>(
                "Character", 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);

        [Header("Control Scheme")]
        #region ControlScheme (FixedString128Bytes)
        // public NetworkVariable<FixedString128Bytes> ControlScheme = 
        //     new NetworkVariable<FixedString128Bytes>(
        //         "ControlScheme", 
        //         NetworkVariableReadPermission.Everyone, 
        //         NetworkVariableWritePermission.Owner);
        #endregion ControlScheme (FixedString128Bytes)
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
        public NetworkVariable<float> HorizontalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<float> VerticalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<float> MoveAmount = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Camera")]
        public NetworkVariable<float> CameraHorizontalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<float> CameraVerticalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerCompositionRoot is null");
            }
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