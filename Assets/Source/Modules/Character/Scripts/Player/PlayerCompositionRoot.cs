using System.Collections;
using Source.Modules.Character.Scripts.Player.StateMachine;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player
{
    public class PlayerCompositionRoot : MonoBehaviour
    {
        [SerializeField] private CharacterNetworkManager _characterNetworkManager;
        [SerializeField] private PlayerCameraMovement _playerCameraMovement;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private GroundChecker _groundChecker;
        private PlayerControls _playerControls;
        private PlayerStateMachine _playerStateMachine;
        private CharacterController _characterController;
        
        public CharacterController CharacterController => _characterController;
        public CharacterNetworkManager CharacterNetworkManager => _characterNetworkManager;
        public PlayerControls PlayerControls => _playerControls;
        public PlayerCameraMovement PlayerCameraMovement => _playerCameraMovement;
        public PlayerView PlayerView => _playerView;
        public PlayerConfig PlayerConfig => _playerConfig;
        public GroundChecker GroundChecker => _groundChecker;

        private void Awake()
        {
            PlayerView.Initialize();
            _characterController = PlayerView.GetComponent<CharacterController>();
            _playerControls = new PlayerControls();
            _playerStateMachine = new PlayerStateMachine(this);
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

        /// <summary>
        /// Этот код полезен, когда нужно временно отключить какое-либо действие ввода, например,
        /// чтобы предотвратить нежелательные повторные действия или для реализации задержек в игре.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
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
    }
}