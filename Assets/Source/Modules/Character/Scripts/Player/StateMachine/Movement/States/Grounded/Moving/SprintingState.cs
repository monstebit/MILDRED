using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        private PlayerConfig _playerConfig;

        public SprintingState(
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
            Data.MovementSpeedModifier = _playerConfig.SprintingStateConfig.SpeedModifier;

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

            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                return;
            }

            StopSprinting();
        }

        #endregion

        private void StopSprinting()
        {
            if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                return;
            }

            StateSwitcher.SwitchState<RunningState>();
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
    }
}