using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        private SprintingStateConfig _sprintingStateConfig;

        private float _startTime;
        private bool _keepSprinting;
        private bool _shouldResetSprintState;
        
        public SprintingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager, 
            PlayerCameraMovement playerPlayerCameraMovement, 
            StateMachineData data) : base(stateSwitcher, 
            playerInputHandler, characterNetworkManager, 
            playerPlayerCameraMovement, 
            data)
        {
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = _sprintingStateConfig.SpeedModifier;
            
            base.Enter();

            PlayerView.StartSprinting();
            
            _startTime = Time.time;
            
            Data.ShouldSprint = true;
            
            if (!Data.ShouldSprint)
            {
                _keepSprinting = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopSprinting();
            
            if (_shouldResetSprintState)
            {
                _keepSprinting = false;
            
                Data.ShouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();

            if (_keepSprinting)
            {
                return;
            }

            if (Time.time < _startTime + _sprintingStateConfig.SprintToRunTime)
            {
                return;
            }
            
            StopSprinting();
        }
        #endregion

        #region MAIN METHODS
        private void StopSprinting()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
            }
            else if (Data.MoveAmount < 0.5f)
            {
                // Если скорость движения низкая, переключаемся в состояние ходьбы
                StateSwitcher.SwitchState<WalkingState>();
            }
            else if (Data.MoveAmount > 0.5 && Data.MoveAmount <= 1)
            {
                // Если скорость движения выше порогового значения, переключаемся в состояние бега
                StateSwitcher.SwitchState<RunningState>();
            }
        }
        #endregion
        
        #region REUSABLE METHODS

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerControls.PlayerMovement.Sprint.performed += OnSprintPerformed;
            PlayerControls.PlayerMovement.Sprint.canceled += OnSprintCanceled;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Sprint.performed -= OnSprintPerformed;
            PlayerControls.PlayerMovement.Sprint.canceled -= OnSprintCanceled;
        }
        #endregion

        #region INPUT METHODS
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _keepSprinting = true;
            
            // Data.ShouldSprint = true;
            
            StateSwitcher.SwitchState<SprintingState>();
        }
        
        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
        }
        #endregion
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<RunningState>();
            
            base.OnMovementCanceled(context);
        }
    }
}