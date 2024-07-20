using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        private SprintingStateConfig _sprintingStateConfig;
        private WalkingStateConfig _walkingStateConfig;
        private MovementStateConfig _movementStateConfig;
        
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
            _walkingStateConfig = playerInputHandler.PlayerConfig.WalkingStateConfig;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = _sprintingStateConfig.SpeedModifier;
            
            base.Enter();

            PlayerView.StartSprinting();
            
            // _sprintingStateConfig._startTime = Time.time;
            
            // _sprintingStateConfig.ShouldSprint = true;
            //
            // if (!_sprintingStateConfig.ShouldSprint)
            // {
            //     // _sprintingStateConfig._keepSprinting = false;
            // }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopSprinting();
            
            // if (_sprintingStateConfig._shouldResetSprintState)
            // {
            //     // _sprintingStateConfig._keepSprinting = false;
            //
            //     // _sprintingStateConfig.ShouldSprint = false;
            // }
        }

        public override void Update()
        {
            base.Update();

            // if (_sprintingStateConfig._keepSprinting)
            // {
            //     return;
            // }

            if (_sprintingStateConfig.ShouldSprint)
            {
                return;
            }
            
            // if (Time.time < _sprintingStateConfig._startTime + _sprintingStateConfig.SprintToRunTime)
            // {
            //     return;
            // }
            
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
            else if (_movementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
            }
            else
            {
                StateSwitcher.SwitchState<RunningState>();
            }
            // else if (Data.MoveAmount < 0.5f)
            // {
            //     // Если скорость движения низкая, переключаемся в состояние ходьбы
            //     StateSwitcher.SwitchState<WalkingState>();
            // }
            // else if (Data.MoveAmount > 0.5 && Data.MoveAmount <= 1)
            // {
            //     // Если скорость движения выше порогового значения, переключаемся в состояние бега
            //     StateSwitcher.SwitchState<RunningState>();
            // }
            
            // StateSwitcher.SwitchState<WalkingState>();
        }
        #endregion
        
        #region REUSABLE METHODS
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }
        #endregion
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
        
        //  SPRINT
        protected override void OnSprintPerformed(InputAction.CallbackContext context)
        {
            base.OnSprintPerformed(context);
        }
        
        protected override void OnSprintCanceled(InputAction.CallbackContext context)
        {
            base.OnSprintCanceled(context);
        }
    }
}