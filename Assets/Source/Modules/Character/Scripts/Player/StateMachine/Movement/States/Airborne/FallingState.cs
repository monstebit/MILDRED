using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class FallingState : AirborneState
    {
        private readonly GroundChecker _groundChecker;

        public FallingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _groundChecker = playerCompositionRoot.GroundChecker;
        }

        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = 0f;
            Data.JumpModifier = 0f;
            
            // PlayerView.StartFalling();
            PlayerView.UpdateState("IsFalling", true);
        }

        public override void Exit()
        {
            base.Exit();
            
            // PlayerView.StopFalling();
            PlayerView.UpdateState("IsFalling", false);
        }
        
        public override void Update()
        {
            base.Update();

            CheckGroundedAndSwitchState();
        }
        
        private void CheckGroundedAndSwitchState()
        {
            if (_groundChecker.isTouches)
            {
                if (InAnimationTransition())
                {
                    return;
                }
        
                StateSwitcher.SwitchState<LightLandingState>();
            }
        }
    }
}