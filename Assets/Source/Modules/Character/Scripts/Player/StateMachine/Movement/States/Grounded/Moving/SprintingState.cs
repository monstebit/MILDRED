using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        private MovementStateConfig _movementStateConfig;
        private SprintingStateConfig _sprintingStateConfig;
        
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
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = _sprintingStateConfig.SpeedModifier;
            
            base.Enter();

            PlayerView.StartSprinting();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopSprinting();
        }

        public override void Update()
        {
            base.Update();

            if (_movementStateConfig.ShouldSprint)
            {
                return;
            }
            
            StopSprinting();
        }
        #endregion

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
        }
    }
}