using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class WalkingState : MovingState
    {
        private WalkingStateConfig _walkingStateConfig;
        private SprintingStateConfig _sprintingStateConfig;
        private MovementStateConfig _movementStateConfig;

        public WalkingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerCameraMovement,
            data)
        {
            _walkingStateConfig = playerInputHandler.PlayerConfig.WalkingStateConfig;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _walkingStateConfig.SpeedModifier; ;
            
            PlayerView.StartWalking();
        }

        public override void Update()
        {
            base.Update();

            if (_movementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
            }

            if (_movementStateConfig.ShouldWalk)
            {
                return;
            }
            
            StopWalking();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopWalking();
        }
        #endregion
        
        // protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        // {
        //     base.OnWalkToggleStarted(context);
        //     
        //     StateSwitcher.SwitchState<RunningState>();
        // }
        
        // protected override void OnWalkToggleCanceled(InputAction.CallbackContext context)
        // {
        //     base.OnWalkToggleStarted(context);
        //     
        //     StateSwitcher.SwitchState<RunningState>();
        // }
        
        private void StopWalking()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
            }
            else
            {
                StateSwitcher.SwitchState<RunningState>();
            }
        }
    }
}