using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class FallingState : AirborneState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly GroundChecker _groundChecker;
        private readonly MovementStateConfig _movementStateConfig;

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
            _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
        }

        public override void Enter()
        {
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
            
            //  ЭТОТ МЕТОД ИСПОЛЬЗУЕТСЯ ДЛЯ ОТМЕНЫ ПРЕРЫВАНИЯ ПРЕЗЕМЛЕНИЯ
            if (InAnimationTransition())
            {
                return;
            }
            
            if (_groundChecker.isTouches)
            {
                #region ВАРИАНТ БЕЗ ПРИЗЕМЛЕНИЯ
                // _movementStateConfig.YVelocity.y = _movementStateConfig.GroundedGravityForce;   //  "ПРИЛИПАНИЕ" К ЗЕМЛЕ
                // if (Data.MovementInput == Vector2.zero)
                // {
                //     StateSwitcher.SwitchState<IdlingState>();
                //     return;
                // }
                // OnMove();
                #endregion
                StateSwitcher.SwitchState<LandingState>();
            }
        }

        public void OnMove()
        {
            if (_movementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                return;
            }
            
            if (_movementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
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