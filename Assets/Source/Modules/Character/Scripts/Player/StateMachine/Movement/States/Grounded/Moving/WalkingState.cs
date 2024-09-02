using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class WalkingState : MovingState
    {
        private PlayerConfig _playerConfig;

        public WalkingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _playerConfig.WalkingStateConfig.SpeedModifier; ;
            
            PlayerView.StartWalking();
        }

        public override void Update()
        {
            base.Update();

            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
            }

            if (_playerConfig.MovementStateConfig.ShouldWalk)
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