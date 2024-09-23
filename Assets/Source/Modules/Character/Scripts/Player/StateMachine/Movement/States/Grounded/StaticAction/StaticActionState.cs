using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class StaticActionState : GroundedState
    {
        protected StaticActionState(
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
            
            PlayerView.StartStaticAction();
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopStaticAction();
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        protected override void Move()
        {
        }
        
        protected override void Rotate()
        {
        }
        
        protected override void CheckGroundAndSwitchToFalling()
        {
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        
        protected override void OnBackStepped(InputAction.CallbackContext context)
        {
        }
        
        protected override void OnDodgeStarted(InputAction.CallbackContext context)
        {
        }
    }
}
