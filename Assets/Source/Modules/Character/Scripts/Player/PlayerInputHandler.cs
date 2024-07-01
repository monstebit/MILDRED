using Source.Modules.Character.Scripts.Player.StateMachine;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        private CharacterNetworkManager characterNetworkManager;
        
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private PlayerView _playerView;
        
        private PlayerControls _playerControls;
        private PlayerStateMachine _playerStateMachine;
        private CharacterController _characterController;
        
        public PlayerControls PlayerControls => _playerControls;
        public CharacterController CharacterController => _characterController;
        public PlayerConfig PlayerConfig => _playerConfig;
        public PlayerView PlayerView => _playerView;
        
        private void Awake()
        {
            PlayerView.Initialize();
            
            _characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            _playerControls = new PlayerControls();
            _playerStateMachine = new PlayerStateMachine(this, this.characterNetworkManager);
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
