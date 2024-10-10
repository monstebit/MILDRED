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
        [SerializeField] private PlayerInput _playerInput;
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
        public PlayerInput PlayerInput => _playerInput;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            //  INIT PLAYER CONTROL SCHEME FOR EACH PLAYER (OLD INPUT)
            if (IsOwner)
            {
                _playerInput = GetComponent<PlayerInput>();
                _playerInput.enabled = true;
            }
        }

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
            DisableNonLocalPlayerCamera();
            
            _playerStateMachine.HandleInput();
            _playerStateMachine.Update();

            PlayerView.UpdateAnimatorMovementParameters();
            PlayerView.UpdateNetworkTransform();
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
    }
}