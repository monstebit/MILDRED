using Source.Modules.Character.Scripts.Player.StateMachine;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        private CharacterNetworkManager _characterNetworkManager;
        
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private GroundChecker _groundChecker;
        
        private PlayerControls _playerControls;
        private PlayerStateMachine _playerStateMachine;
        private CharacterController _characterController;
        // public CharacterController _characterController;
        
        public PlayerControls PlayerControls => _playerControls;
        public CharacterController CharacterController => _characterController;
        public PlayerConfig PlayerConfig => _playerConfig;
        public PlayerView PlayerView => _playerView;
        public GroundChecker GroundChecker => _groundChecker;
        
        private void Awake()
        {
            PlayerView.Initialize();
            
            _characterController = GetComponent<CharacterController>();
            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
            _playerControls = new PlayerControls();
            _playerStateMachine = new PlayerStateMachine(this, this._characterNetworkManager, this._cameraMovement);
        }

        private void Update()
        {
            _playerStateMachine.HandleInput();
            _playerStateMachine.Update();
        }
        
        private void LateUpdate()
        {
            _playerStateMachine.LateUpdate();
        }
        
        private void OnEnable() => _playerControls.Enable();
        
        private void OnDisable() => _playerControls.Disable();
    }
}