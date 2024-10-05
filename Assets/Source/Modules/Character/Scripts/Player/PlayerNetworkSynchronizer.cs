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
        // public NetworkVariable<FixedString128Bytes> ControlScheme = 
        //     new NetworkVariable<FixedString128Bytes>(
        //         "ControlScheme", 
        //         NetworkVariableReadPermission.Everyone, 
        //         NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> ControlScheme = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
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
        
        [Header("Animator")]
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

        [Header("Flags")]
        public NetworkVariable<bool> IsGrounded = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsMoving = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsStaticAction = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsAirborne = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsLanding = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<bool> IsIdling = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsWalking = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); 
        public NetworkVariable<bool> IsRunning = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); 
        public NetworkVariable<bool> IsSprinting = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); 
        
        public NetworkVariable<bool> IsDodging = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsBackStepping = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<bool> IsLightLanding = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<bool> IsJumping = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsFalling = 
            new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerCompositionRoot is null");
            }
        }
    }
}