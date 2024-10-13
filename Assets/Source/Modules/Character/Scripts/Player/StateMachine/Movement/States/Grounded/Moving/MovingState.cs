using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class MovingState : GroundedState
    {
        public MovingState(
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
            
            PlayerView.UpdateState("IsMoving", true);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.UpdateState("IsMoving", false);
        }

        public override void Update()
        {
            base.Update();
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
        
        private void ResetShouldSprint()
        {
            // PlayerConfig.MovementStateConfig.ShouldSprint = false;
        }
    }
}