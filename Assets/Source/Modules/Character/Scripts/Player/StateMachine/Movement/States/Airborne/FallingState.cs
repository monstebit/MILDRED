using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class FallingState : AirborneState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly GroundChecker _groundChecker;

        public FallingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _groundChecker = playerCompositionRoot.GroundChecker;
        }

        public override void Enter()
        {
            // _playerCompositionRoot.PlayerView.StartActionAnimation("core_main_jump_01_lift");
            
            base.Enter();

            Data.MovementSpeedModifier = 1; //  ON TESTING
            
            PlayerView.StartFalling();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopFalling();
        }
        
        public override void Update()
        {
            base.Update();
            
            if (_groundChecker.isTouches)
            {
                if (InAnimationTransition())
                {
                    return;
                }
                
                StateSwitcher.SwitchState<LightLandingState>();
            }
        }
        
        protected override void ResetPerformingAction()
        {
            base.ResetPerformingAction();
        }
        
        #region OnAmimationEvent Methods
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
        #endregion
    }
}