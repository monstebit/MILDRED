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
        [SerializeField] private CharacterController _characterController;
        
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
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            // PlayerView.Initialize();
            _playerControls = new PlayerControls();
            _playerStateMachine = new PlayerStateMachine(this);
        }
        
        public override void OnNetworkSpawn()
        {
            // base.OnNetworkSpawn();
            if (IsOwner)
            {
                InitControlSchemePlayerInput();
            }
        }
        
        private void Update()
        {
            DisableNonLocalPlayerCamera();
            
            _playerStateMachine.HandleInput();
            _playerStateMachine.Update();
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
    }
}