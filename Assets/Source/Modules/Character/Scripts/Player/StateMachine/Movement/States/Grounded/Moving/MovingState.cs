using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
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
            
            PlayerView.StartMoving();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopMoving();
        }

        public override void Update()
        {
            base.Update();
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
    }
}