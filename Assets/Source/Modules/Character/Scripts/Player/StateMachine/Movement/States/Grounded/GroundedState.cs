using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private PlayerInputHandler _playerInputHandler;
        private PlayerConfig _playerConfig;
        
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
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartGrounded();
            
            UpdateShouldSprintState();
            // UpdateShouldDodgeState();
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
            Debug.Log("[ Сброс СПРИНТА ]");
        }
        
        private void UpdateShouldDodgeState()
        {
            _playerConfig.MovementStateConfig.IsPerformingAction = false;
            Debug.Log("[ Сброс ДОДЖА ]");
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
        
        #region REUSABLE METHODS
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerControls.PlayerMovement.Dodge.started += OnDodgeStarted;
            PlayerControls.PlayerMovement.Jump.started += OnJumpStarted;
            PlayerControls.PlayerMovement.Sprint.performed += OnSprintPerformed;
            PlayerControls.PlayerMovement.Sprint.canceled += OnSprintCanceled;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Dodge.started -= OnDodgeStarted;
            PlayerControls.PlayerMovement.Jump.started -= OnJumpStarted;
            PlayerControls.PlayerMovement.Sprint.performed -= OnSprintPerformed;
            PlayerControls.PlayerMovement.Sprint.canceled -= OnSprintCanceled;
        }
        
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
        #endregion
        
        #region INPUT METHODS
        protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
        {
            if (Data.MovementInput == Vector2.zero)
                return;
            
            if (_playerConfig.MovementStateConfig.IsPerformingAction)
                return;
            
            _playerConfig.MovementStateConfig.IsPerformingAction = true;
            
            StateSwitcher.SwitchState<DodgingState>();
        }
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (_playerConfig.MovementStateConfig.IsPerformingAction)
                return;
            
            StateSwitcher.SwitchState<JumpingState>();
            
            // _playerConfig.MovementStateConfig.IsPerformingAction = true;
        }
        
        protected virtual void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig.ShouldSprint = true;
        }
        
        protected virtual void OnSprintCanceled(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig.ShouldSprint = false;
        }
        #endregion
    }
}