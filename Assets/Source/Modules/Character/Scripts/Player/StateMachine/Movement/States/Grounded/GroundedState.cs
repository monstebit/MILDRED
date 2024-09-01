using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private PlayerInputHandler _playerInputHandler;
        private PlayerConfig _playerConfig;
        
        private DodgeStateConfig _dodgeStateConfig;
        private MovementStateConfig _movementStateConfig;
        
        public GroundedState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerPlayerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerPlayerCameraMovement,
            data)
        {
            _playerConfig = playerInputHandler.PlayerConfig;
            _playerInputHandler = playerInputHandler;
            _dodgeStateConfig = _playerConfig.DodgeStateConfig;
            _movementStateConfig = _playerConfig.MovementStateConfig;
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartGrounded();
            
            UpdateShouldSprintState();
        }
        
        private void UpdateShouldSprintState()
        {
            if (!_playerConfig.MovementStateConfig.ShouldSprint)
            {
                return;
            }
            
            if (Data.MovementInput != Vector2.zero)
            {
                return;
            }
            
            _playerConfig.MovementStateConfig.ShouldSprint = false;
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopGrounded();
        }

        public override void Update()
        {
            base.Update();
            
            if (_playerInputHandler.GroundChecker.isTouches == false)
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }
        #endregion
        
        protected void OnMove()
        {
            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                
                return;
            }
            
            if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
        
        #region REUSABLE METHODS
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            
            PlayerControls.Player.Jump.performed += OnJumpStarted;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.Player.Jump.performed -= OnJumpStarted;
        }
        #endregion
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (_playerConfig.MovementStateConfig.IsPerformingAction)
            {
                return;
            }
            
            StateSwitcher.SwitchState<JumpingState>();
        }
    }
}