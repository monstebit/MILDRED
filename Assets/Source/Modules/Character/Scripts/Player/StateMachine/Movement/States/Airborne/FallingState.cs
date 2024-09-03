using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class FallingState : AirborneState
    {
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
            _groundChecker = playerCompositionRoot.GroundChecker;
            _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
        }

        public override void Enter()
        {
            base.Enter();

            // Data.MovementSpeedModifier = 0;
            Data.MovementSpeedModifier = 1;
            
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
                StateSwitcher.SwitchState<LandingState>();  //  ON TESTING
                
                //  TODO: CHECK THIS BEFORE DELETE!!
                // _movementStateConfig.YVelocity.y = _movementStateConfig.GroundedGravityForce;   //  "ПРИЛИПАНИЕ" К ЗЕМЛЕ
                // if (Data.MovementInput == Vector2.zero)
                // {
                //     StateSwitcher.SwitchState<IdlingState>();
                //     return;
                // }
                //
                // OnMove();
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
    }
}