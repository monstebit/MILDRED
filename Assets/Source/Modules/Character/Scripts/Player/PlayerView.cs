using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : NetworkBehaviour
    {
        private readonly HashSet<string> _activeStates = new();
        
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        [SerializeField] private Vector2 _defaultInitialPosition = new Vector2(-4, 4);
        PlayerNetworkSynchronizer _playerNetworkSynchronizer;
        
        [SerializeField] public Animator Animator;

        private void Awake()
        {
            _playerNetworkSynchronizer = _playerCompositionRoot.PlayerNetworkSynchronizer;
        }

        private void Start()
        {
            InitializePlayerPosition();
        }

        private void Update()
        {
            UpdateAnimatorMovementParameters();
        }

        private void InitializePlayerPosition()
        {
            // if (IsClient && IsOwner)
            // {
            //     transform.position = new Vector3(Random.Range(
            //             _defaultInitialPosition.x, _defaultInitialPosition.y), 0,
            //         Random.Range(_defaultInitialPosition.x, _defaultInitialPosition.y));
            // }
            transform.position = new Vector3(Random.Range(
                    _defaultInitialPosition.x, _defaultInitialPosition.y), 0,
                Random.Range(_defaultInitialPosition.x, _defaultInitialPosition.y));
        }
        
        //  PLAYER ANIMATION UPDATERS
        private void UpdateAnimatorMovementParameters()
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
            if (IsOwner == false) return;
            
            bool isStateActive = _activeStates.Contains(stateName);

            // Если новое состояние отличается от текущего
            if (newState && !isStateActive)
            {
                _activeStates.Add(stateName); // Добавляем активное состояние
                _playerNetworkSynchronizer.UpdateAnimationStateServerRpc(stateName, true);
            }
            else if (!newState && isStateActive)
            {
                _activeStates.Remove(stateName); // Убираем неактивное состояние
                _playerNetworkSynchronizer.UpdateAnimationStateServerRpc(stateName, false);
            }
        }
    }
}