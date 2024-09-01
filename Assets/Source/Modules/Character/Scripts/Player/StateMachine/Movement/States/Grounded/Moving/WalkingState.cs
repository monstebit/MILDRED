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
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _walkingStateConfig = playerCompositionRoot.PlayerConfig.WalkingStateConfig;
            _sprintingStateConfig = playerCompositionRoot.PlayerConfig.SprintingStateConfig;
            _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
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