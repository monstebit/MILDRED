using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;

        public SprintingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot,
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }
        
        public override void Enter()
        {
            Data.MovementSpeedModifier = _playerConfig.SprintingStateConfig.SpeedModifier;
            Data.JumpModifier = 1.75f;

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
        
        private void StopSprinting()
        {
            if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                return;
            }

            StateSwitcher.SwitchState<RunningState>();
        }
    }
}