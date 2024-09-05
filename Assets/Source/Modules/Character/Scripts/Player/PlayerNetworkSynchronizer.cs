using Unity.Netcode;
using Unity.Collections;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerNetworkSynchronizer : NetworkBehaviour
    {
        [Header("Player Name")] 
        public NetworkVariable<FixedString64Bytes> characterName = 
            new NetworkVariable<FixedString64Bytes>(
                "Character", 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        
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
        
        [Header("Movement")]
        public NetworkVariable<float> HorizontalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<float> VerticalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Camera")]
        public NetworkVariable<float> CameraHorizontalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<float> CameraVerticalMovement = 
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }
}