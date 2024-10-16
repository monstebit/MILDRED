using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing
{
    public class LightLandingState : LandingState
    {
        public LightLandingState(
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

            Data.MovementSpeedModifier = 0f;
            
            PlayerView.UpdateState("IsLightLanding", true);
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.UpdateState("IsLightLanding", false);
        }
        
        public override void Update()
        {
            base.Update();
            
            if (Data.MovementInput == Vector2.zero)
            {
                if (InAnimationTransition())
                {
                    return;
                }
                
                StateSwitcher.SwitchState<IdlingState>();
                return;
            }
            
            OnMove();
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            // base.OnMovementCanceled(context);
        }
        
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        
        #region OnAmimationEvent Methods
        // private bool InAnimationTransition(int layerIndex = 0)
        // {
        //     return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        // }
        #endregion
    }
}