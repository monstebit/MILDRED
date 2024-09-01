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
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
            _sprintingStateConfig = playerCompositionRoot.PlayerConfig.SprintingStateConfig;
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
                
                return;
            }
            
            if (_movementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
    }
}