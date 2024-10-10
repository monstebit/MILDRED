using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing
{
    public class LandingState : GroundedState
    {
        protected LandingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            // PlayerView.StartLanding();
            PlayerView.UpdateState("IsLanding", true);
        }

        public override void Exit()
        {
            base.Exit();
            
            // PlayerView.StopLanding();
            PlayerView.UpdateState("IsLanding", false);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}