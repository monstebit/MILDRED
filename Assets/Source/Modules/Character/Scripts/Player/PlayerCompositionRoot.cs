using System;
using System.Collections;
using Source.Modules.Character.Scripts.Player.StateMachine;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerCompositionRoot : NetworkBehaviour
    {
        private Vector3 playerPos;
        //
        [SerializeField] private PlayerNetworkSynchronizer _playerNetworkSynchronizer;
        [SerializeField] private PlayerCameraMovement _playerCameraMovement;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Vector2 _defaultInitialPosition = new Vector2(-4, 4);
        
        private PlayerControls _playerControls;
        private PlayerStateMachine _playerStateMachine;
        
        public PlayerNetworkSynchronizer PlayerNetworkSynchronizer => _playerNetworkSynchronizer;
        public CharacterController CharacterController => _characterController;
        public PlayerControls PlayerControls => _playerControls;
        public PlayerCameraMovement PlayerCameraMovement => _playerCameraMovement;
        public PlayerView PlayerView => _playerView;
        public PlayerConfig PlayerConfig => _playerConfig;
        public GroundChecker GroundChecker => _groundChecker;
        public PlayerInput PlayerInput => _playerInput;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsOwner)
            {
                InitControlSchemePlayerInput();
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            // PlayerView.Initialize();
            _playerControls = new PlayerControls();
            _playerStateMachine = new PlayerStateMachine(this);
        }
        
        private void Update()
        {
            DisableNonLocalPlayerCamera();
            
            //  RPC METHOD
            if (IsOwner && IsClient)
            {
                UpdateClientPositionAndRotationServerRpc(PlayerView.transform.position, PlayerView.transform.rotation);
                // UpdatePosition();
            }
            //  RPC METHOD
            
            _playerStateMachine.HandleInput();
            _playerStateMachine.Update();
            //  NETWORK
            PlayerView.UpdateAnimatorMovementParameters();
            // UpdateNetworkTransform();   //  NetworkVariable METHOD
            //  NETWORK
        }
        
        private void LateUpdate()
        {
            _playerStateMachine.LateUpdate();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }
        
        private void OnDisable() => _playerControls.Disable();

        private void DisableNonLocalPlayerCamera()
        {
            if (IsLocalPlayer == false)
            {
                PlayerCameraMovement.Camera.enabled = false;
            }
        }
        
        private void InitControlSchemePlayerInput()
        {
            _playerInput = gameObject.AddComponent<PlayerInput>();
            _playerInput.enabled = false;
            _playerInput.actions = _playerControls.asset;
            _playerInput.enabled = true;
        }
        
        public void OnMovementStateAnimationEnterEvent()
        {
            _playerStateMachine.OnAnimationEnterEvent();
        }

        public void OnMovementStateAnimationExitEvent()
        {
            _playerStateMachine.OnAnimationExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            _playerStateMachine.OnAnimationTransitionEvent();
        }
        
        #region DisableActionFor METHOD
        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
        #endregion DisableActionFor
        
        //  RPC METHOD
        [ServerRpc]
        public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Quaternion newRotation)
        {
            // Обновляем данные на сервере
            PlayerNetworkSynchronizer.NetworkPosition.Value = newPosition;
            PlayerNetworkSynchronizer.NetworkRotation.Value = newRotation;
            
            // Отправляем данные всем клиентам
            UpdateClientPositionAndRotationClientRpc(newPosition, newRotation);
        }
        
        [ClientRpc]
        public void UpdateClientPositionAndRotationClientRpc(Vector3 newPosition, Quaternion newRotation)
        {
            // Обновляем позицию и поворот на всех клиентах
            if (IsOwner) return; // Пропускаем владельца, так как он уже обновил свои данные
            PlayerView.transform.position = newPosition;
            PlayerView.transform.rotation = newRotation;
        }
        
        //  NetworkVariable METHOD
        public void UpdateNetworkTransform()
        {
            var playerNetworkSynchronizer = PlayerNetworkSynchronizer;
            var playerView = PlayerView;
            
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
    }
}