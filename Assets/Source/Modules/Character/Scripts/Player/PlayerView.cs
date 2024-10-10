using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        // private const string IsGrounded = "IsGrounded";
        // private const string IsMoving = "IsMoving";
        // private const string IsStaticAction = "IsStaticAction";
        // private const string IsLanding = "IsLanding";
        // private const string IsAirborne = "IsAirborne";
        // private const string IsIdling = "IsIdling";
        // private const string IsWalking = "IsWalking";
        // private const string IsRunning = "IsRunning";
        // private const string IsSprinting = "IsSprinting";
        // private const string IsDodging = "IsDodging";
        // private const string IsBackStepping = "IsBackStepping";
        // private const string IsLightLanding = "IsLightLanding";
        // private const string IsJumping = "IsJumping";
        // private const string IsFalling = "IsFalling";
        
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        private bool IsOwner => _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner; 
        
        // Словарь для хранения состояний
        // private Dictionary<string, bool> _animationStates = new Dictionary<string, bool>()
        // {
        //     { "IsGrounded", false },
        //     { "IsMoving", false },
        //     { "IsStaticAction", false },
        //     { "IsLanding", false },
        //     { "IsAirborne", false },
        //     { "IsIdling", false },
        //     { "IsWalking", false },
        //     { "IsRunning", false },
        //     { "IsSprinting", false },
        //     { "IsDodging", false },
        //     { "IsBackStepping", false },
        //     { "IsLightLanding", false },
        //     { "IsJumping", false },
        //     { "IsFalling", false },
        // };
        private HashSet<string> _activeStates = new HashSet<string>();
        
        public Animator Animator;
        
        public void Initialize() => Animator = GetComponent<Animator>();
        
        public void UpdateNetworkTransform()
        {
            var playerNetworkSynchronizer = _playerCompositionRoot.PlayerNetworkSynchronizer;
            var playerView = _playerCompositionRoot.PlayerView;
            
            if (IsOwner)
            {
                playerNetworkSynchronizer.NetworkPosition.Value = playerView.transform.position;
                playerNetworkSynchronizer.NetworkRotation.Value = playerView.transform.rotation;
            }
            else
            {
                playerView.transform.position = Vector3.SmoothDamp(
                    playerView.transform.position,
                    playerNetworkSynchronizer.NetworkPosition.Value,
                    ref playerNetworkSynchronizer.NetworkPositionVelocity,
                    playerNetworkSynchronizer.NetworkPositionSmoothTime);

                playerView.transform.rotation = Quaternion.Slerp(
                    playerView.transform.rotation,
                    playerNetworkSynchronizer.NetworkRotation.Value,
                    playerNetworkSynchronizer.NetworkRotationSmoothTime);
            }
        }
        
        public void UpdateAnimatorMovementParameters()
        {
            int vertical = Animator.StringToHash("Vertical");
            int horizontal = Animator.StringToHash("Horizontal");
            Animator.SetFloat(horizontal, 0f, 0.1f, Time.deltaTime);
            Animator.SetFloat(
                vertical, 
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value, 
                0.1f, 
                Time.deltaTime);
        }
        
        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerView: No player composition root");
            }
        }
        
        // public void UpdateState(string stateName, bool newState)
        // {
        //     if (IsOwner && _animationStates.ContainsKey(stateName))
        //     {
        //         if (_animationStates[stateName] != newState) // Проверяем, изменилось ли состояние
        //         {
        //             _animationStates[stateName] = newState; // Обновляем текущее состояние
        //             _playerCompositionRoot.PlayerNetworkSynchronizer.UpdateAnimationStateServerRpc(
        //                 stateName, newState);
        //         }
        //     }
        // }
        public void UpdateState(string stateName, bool newState)
        {
            if (IsOwner)
            {
                bool isStateActive = _activeStates.Contains(stateName);

                // Если новое состояние отличается от текущего
                if (newState && !isStateActive)
                {
                    _activeStates.Add(stateName); // Добавляем активное состояние
                    _playerCompositionRoot.PlayerNetworkSynchronizer.UpdateAnimationStateServerRpc(
                        stateName, true);
                }
                else if (!newState && isStateActive)
                {
                    _activeStates.Remove(stateName); // Убираем неактивное состояние
                    _playerCompositionRoot.PlayerNetworkSynchronizer.UpdateAnimationStateServerRpc(
                        stateName, false);
                }
            }
        }
    }
}