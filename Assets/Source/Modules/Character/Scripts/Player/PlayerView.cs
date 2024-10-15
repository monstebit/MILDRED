using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : NetworkBehaviour
    {
        // public void Initialize() => Animator = GetComponent<Animator>();
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        [SerializeField] private Vector2 _defaultInitialPosition = new Vector2(-4, 4);
        [SerializeField] public Animator Animator;
        
        private HashSet<string> _activeStates = new();
        
        // //  ON TESTING
        // private void Start()
        // {
        //     if (IsClient && IsOwner)
        //     {
        //         transform.position = new Vector3(Random.Range(
        //                 _defaultInitialPosition.x, _defaultInitialPosition.y), 0,
        //             Random.Range(_defaultInitialPosition.x, _defaultInitialPosition.y));
        //     }
        // }
        // //  ON TESTING
        
        private bool IsOwner => _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner; 
        
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
                    UpdateAnimationStateServerRpc(
                        stateName, true);
                }
                else if (!newState && isStateActive)
                {
                    _activeStates.Remove(stateName); // Убираем неактивное состояние
                    UpdateAnimationStateServerRpc(
                        stateName, false);
                }
            }
        }
        
        //  NETWORK
        [ServerRpc]
        public void UpdateAnimationStateServerRpc(string stateName, bool state)
        {
            // Если это сервер, то инициируем обновление состояния на клиентах
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
        //  NETWORK
    }
}