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
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartGrounded();
            
            UpdateShouldSprintState();
        }
        
        /// <summary>
        /// Здесь проверяется, если MovementInput равен Vector2.zero
        /// (персонаж не движется), то ShouldSprint сбрасывается в false,
        /// прекращая спринт.
        /// </summary>
        private void UpdateShouldSprintState()
        {
            if (_playerConfig.MovementStateConfig.ShouldSprint)
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

            PlayerControls.PlayerMovement.Sprint.performed += OnSprintPerformed;
            PlayerControls.PlayerMovement.Sprint.canceled += OnSprintCanceled;
            PlayerControls.PlayerMovement.Dodge.performed += OnDodgeStarted;
            PlayerControls.PlayerMovement.Jump.performed += OnJumpStarted;
            PlayerControls.PlayerMovement.BackStep.performed += OnBackStepped;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Sprint.performed -= OnSprintPerformed;
            PlayerControls.PlayerMovement.Sprint.canceled -= OnSprintCanceled;
            PlayerControls.PlayerMovement.Dodge.performed -= OnDodgeStarted;
            PlayerControls.PlayerMovement.Jump.performed -= OnJumpStarted;
            PlayerControls.PlayerMovement.BackStep.performed -= OnBackStepped;
        }
        #endregion
        
        protected virtual void OnBackStepped(InputAction.CallbackContext context)
        {
            if (Data.MovementInput != Vector2.zero)
                return;
            
            if (_playerConfig.MovementStateConfig.IsPerformingAction)
                return;
            
            StateSwitcher.SwitchState<BackSteppingState>();
        }
        
        protected virtual void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig.ShouldSprint = true;
        }
        
        protected virtual void OnSprintCanceled(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig.ShouldSprint = false;
        }
        
        private void DodgingTimer()
        {
            if (_dodgeStateConfig._dodgingTimer >= 0)
            {
                _dodgeStateConfig._dodgingTimer -= Time.deltaTime;
            }
        }
        
        protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
        {
            if (Data.MovementInput == Vector2.zero)
            {
                return;
            }

            if (_playerConfig.MovementStateConfig.IsPerformingAction)
            {
                return;
            }
            
            if (_dodgeStateConfig._dodgingTimer <= 0)
            {
                // _dodgeStateConfig._dodgingTimer = 0.4f;
                _dodgeStateConfig._dodgingTimer = 0.2f;
                
                return;
            }
            
            StateSwitcher.SwitchState<DodgingState>();
        }
        
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