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
            
            DodgingTimer();
            
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

            // PlayerControls.PlayerMovement.Dodge.performed += OnDodgeStarted;
            // PlayerControls.PlayerMovement.Dodge.started += OnDodgeStarted;
            // PlayerControls.PlayerMovement.BackStep.performed += OnBackStepped;
            PlayerControls.PlayerMovement.Jump.performed += OnJumpStarted;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            // PlayerControls.PlayerMovement.Dodge.performed -= OnDodgeStarted;
            // PlayerControls.PlayerMovement.Dodge.started -= OnDodgeStarted;
            // PlayerControls.PlayerMovement.BackStep.performed -= OnBackStepped;
            PlayerControls.PlayerMovement.Jump.performed -= OnJumpStarted;
        }
        #endregion
        
        protected virtual void OnBackStepped(InputAction.CallbackContext context)
        {
            if (_playerConfig.MovementStateConfig.IsPerformingAction)
            {
                return;
            }
            
            if (Data.MovementInput != Vector2.zero)
            {
                return;
            }
            
            StateSwitcher.SwitchState<BackSteppingState>();
        }
        
        private void DodgingTimer()
        {
            // if (_dodgeStateConfig._dodgingTimer >= 0)
            // {
            //     _dodgeStateConfig._dodgingTimer -= Time.deltaTime;
            // }
        }
        
        protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
        {
            _dodgeStateConfig._dodgingTimer = 0f;
            
            //  TEST
            if (Data.MovementInput == Vector2.zero)
            {
                return;
            }
            
            if (_playerConfig.MovementStateConfig.IsPerformingAction)
            {
                return;
            }
            
            // if (_dodgeStateConfig._dodgingTimer <= 0)
            // {
            //     _dodgeStateConfig._dodgingTimer = 0.25f;
            //     
            //     return;
            // }
            //  END TEST

            
            
            
            
            StateSwitcher.SwitchState<DodgingState>();
        }
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            //  TEST
            // if (_playerConfig.MovementStateConfig.IsPerformingAction)
            // {
            //     return;
            // }
            
            StateSwitcher.SwitchState<JumpingState>();
        }
    }
}