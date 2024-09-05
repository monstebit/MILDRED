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
            
            PlayerView.StartLightLanding();
        }

        public override void Update()
        {
            base.Update();
            
            // if (Data.MovementInput == Vector2.zero)
            // {
            //     StateSwitcher.SwitchState<IdlingState>();
            //     {
            //         return;
            //     }
            // }
            //
            // OnMove();
            
            if (Data.MovementInput != Vector2.zero)
            {
                OnMove();
                
                return;
            }
            
            StateSwitcher.SwitchState<IdlingState>();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopLightLanding();
        }
        
        //  ON TESTING
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
        
        //  ON TESTING
        // public override void OnAnimationTransitionEvent()
        // {
        //     StateSwitcher.SwitchState<IdlingState>();
        //     Debug.Log("===OnAnimationTransitionEvent===");
        // }
    }
}