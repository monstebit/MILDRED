using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        // [SerializeField] public TextMeshProUGUI playerName;
        // [SerializeField] public Canvas _canvas;
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        private HashSet<string> _activeStates = new();
        
        public Animator Animator;
        
        private bool IsOwner => _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner; 
        
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