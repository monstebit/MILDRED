using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing
{
    public class LandingState : GroundedState
    {
        public LandingState(
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
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
    }
}