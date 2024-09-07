using System.Collections;
using Source.Modules.Character.Scripts.Player.StateMachine;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerCompositionRoot : NetworkBehaviour
    {
        [SerializeField] private PlayerNetworkSynchronizer _playerNetworkSynchronizer;
        [SerializeField] private PlayerCameraMovement _playerCameraMovement;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private GroundChecker _groundChecker;
        private PlayerControls _playerControls;
        private PlayerStateMachine _playerStateMachine;
        private CharacterController _characterController;
        
        public PlayerNetworkSynchronizer PlayerNetworkSynchronizer => _playerNetworkSynchronizer;
        
        public CharacterController CharacterController => _characterController;
        public PlayerControls PlayerControls => _playerControls;
        public PlayerCameraMovement PlayerCameraMovement => _playerCameraMovement;
        public PlayerView PlayerView => _playerView;
        public PlayerConfig PlayerConfig => _playerConfig;
        public GroundChecker GroundChecker => _groundChecker;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            PlayerView.Initialize();
            
            _characterController = PlayerView.GetComponent<CharacterController>();
            
            _playerControls = new PlayerControls();
            _playerStateMachine = new PlayerStateMachine(this);
            
            _playerNetworkSynchronizer = GetComponent<PlayerNetworkSynchronizer>();
        }

        private void Update()
        {
            //  POSITION FOR EACH PLAYER
            if (IsOwner)
            {
                _playerNetworkSynchronizer.NetworkPosition.Value = _playerView.transform.position;
                _playerNetworkSynchronizer.NetworkRotation.Value = _playerView.transform.rotation;
            }
            else
            {
                _playerView.transform.position = Vector3.SmoothDamp(
                    _playerView.transform.position,
                    _playerNetworkSynchronizer.NetworkPosition.Value,
                    ref _playerNetworkSynchronizer.NetworkPositionVelocity,
                    _playerNetworkSynchronizer.NetworkPositionSmoothTime);

                _playerView.transform.rotation = Quaternion.Slerp(
                    _playerView.transform.rotation,
                    _playerNetworkSynchronizer.NetworkRotation.Value,
                    _playerNetworkSynchronizer.NetworkRotationSmoothTime);
            }
            
            //  CAMERA FOR EACH PLAYER
            if (IsLocalPlayer == false)
            {
                PlayerCameraMovement.Camera.enabled = false;
                
                // return;
            }

            //  IF WE DO NOT OWN THIS GAMEOBJECT, WE NO NOT CONTROL OR EDIT IT
            if (IsOwner == false)
            {
                // return;
            }
            
            _playerStateMachine.HandleInput();
            _playerStateMachine.Update();
        }

        private void LateUpdate()
        {
            //  IF WE DO NOT OWN THIS GAMEOBJECT, WE NO NOT CONTROL OR EDIT IT
            if (IsOwner == false)
            {
                // return;
            }
            
            _playerStateMachine.LateUpdate();
        }

        private void OnEnable() => _playerControls.Enable();

        private void OnDisable() => _playerControls.Disable();
        
        #region OnMovementStateAnimatio
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
        #endregion
        
        #region DisableActionFor
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
        #endregion
    }
}