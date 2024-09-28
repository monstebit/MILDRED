using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using UnityEngine;

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
            base.Enter();

            Data.MovementSpeedModifier = 0f;
            Data.JumpModifier = 0f;
            
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
                
                #region WITHOUT LANDING
                // if (Data.MovementInput == Vector2.zero)
                // {
                //     StateSwitcher.SwitchState<IdlingState>();
                //     return;
                // }
                //
                // OnMove();
                #endregion
            }
        }
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}