using System.Diagnostics;
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
        private PlayerConfig _playerConfig;
        private PlayerInputHandler _playerInputHandler;
        
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

            UpdateShouldSprintState();
            
            PlayerView.StartGrounded();
        }
        
        /// <summary>
        /// Здесь проверяется, если MovementInput равен Vector2.zero
        /// (персонаж не движется), то ShouldSprint сбрасывается в false,
        /// прекращая спринт.
        /// </summary>
        private void UpdateShouldSprintState()
        {
            if (_playerConfig.SprintingStateConfig.ShouldSprint)
            {
                return;
            }
            
            if (Data.MovementInput != Vector2.zero)
            {
                return;
            }
            
            _playerConfig.SprintingStateConfig.ShouldSprint = false;
            Debug.Log("[ Сброс Спринта ]");
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
            if (_playerConfig.SprintingStateConfig.ShouldSprint)
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
            StateSwitcher.SwitchState<DodgingState>();

            _playerConfig.MovementStateConfig.shouldDodge = true;
        }
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<JumpingState>();
            
            _playerConfig.MovementStateConfig.shouldAirborne = true;
        }
        
        //  SPRINT
        protected virtual void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _playerConfig.SprintingStateConfig.ShouldSprint = true;
        }
        
        protected virtual void OnSprintCanceled(InputAction.CallbackContext context)
        {
            _playerConfig.SprintingStateConfig.ShouldSprint = false;
        }
        #endregion
    }
}