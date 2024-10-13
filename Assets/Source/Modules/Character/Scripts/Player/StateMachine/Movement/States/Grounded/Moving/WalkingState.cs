using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

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

        public override void Enter()
        {
            base.Enter();
            
            Data.MovementSpeedModifier = _playerConfig.WalkingStateConfig.SpeedModifier; ;
            Data.JumpModifier = 0.5f;
            
            PlayerView.UpdateState("IsWalking", true);
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.UpdateState("IsWalking", false);
        }

        public override void Update()
        {
            base.Update();
            
            OnMove();
        }
        
        protected override void OnMove()
        {
            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                return;
            }

            if (_playerConfig.MovementStateConfig.ShouldWalk == false)
            {
                StateSwitcher.SwitchState<RunningState>();
            }
        }
    }
}