using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI playerName;
        [SerializeField] public Canvas _canvas;
        public Camera ownerCamera; // Камера, на которую должен смотреть Canvas
        public Vector3 ownerPos; // Камера, на которую должен смотреть Canvas
        //
        [SerializeField] private PlayerCompositionRoot _playerCompositionRoot;
        private bool IsOwner => _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner; 
        
        private HashSet<string> _activeStates = new HashSet<string>();
        
        public Animator Animator;
        public void Initialize() => Animator = GetComponent<Animator>();
        
        
        
        
        
        
        private void Awake()
        {
            if (_playerCompositionRoot == null)
            {
                Debug.LogError("PlayerView: No player composition root");
            }

            ownerPos = _playerCompositionRoot.PlayerNetworkSynchronizer.CameraPosition.Value;

            //  ЕСЛИТ Я OWNER
            // if (IsOwner)
            // {
            //     ownerCamera = GetComponentInChildren<Camera>();
            // }

            // // Проходим по всем подключенным клиентам
            // foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            // {
            //     if (client.ClientId == NetworkManager.Singleton.LocalClientId && client.PlayerObject != null)
            //     {
            //         // Предполагаем, что камера владельца находится в его объекте
            //         ownerCamera = client.PlayerObject.GetComponentInChildren<Camera>();
            //         break;
            //     }
            // }
        }
        
        
        //IsLocalPlayer — это свойство, которое возвращает true,
        //если данный игровой объект (игрок) является локальным игроком на данном клиенте.
        //Локальный игрок — это игрок, который управляется текущим клиентом.
        private void LateUpdate()
        {
            // var localCamera = _playerCompositionRoot.PlayerCameraMovement.Camera;
            //
            // if (!IsOwner) return;
            //
            // foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            // {
            //     if (client.ClientId != NetworkManager.Singleton.LocalClientId && client.PlayerObject != null)
            //     {
            //         var otherPlayerCanvas = client.PlayerObject.GetComponentInChildren<Canvas>();
            //         if (otherPlayerCanvas != null)
            //         {
            //             otherPlayerCanvas.transform.LookAt(localCamera.transform);
            //         }
            //         else
            //         {
            //             Debug.LogWarning("PlayerView: No player canvas");
            //         }
            //     }
            // }
        }

        
        
        
        
        private void Update()
        {
            if (IsOwner)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.characterName.Value = NetworkManager.Singleton.LocalClientId;
                playerName.text = _playerCompositionRoot.PlayerNetworkSynchronizer.characterName.Value.ToString();
            }
            
            playerName.text = _playerCompositionRoot.PlayerNetworkSynchronizer.characterName.Value.ToString();
        }

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