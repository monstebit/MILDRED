using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class StaticActionState : GroundedState
    {
        private PlayerConfig _playerConfig;
        
        protected StaticActionState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.UpdateState("IsStaticAction", true);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.UpdateState("IsStaticAction", false);
        }
        
        public override void Update()
        {
            base.Update();

            ApplyGravity();
        }
        
        private void ApplyGravity()
        {
            _playerConfig.MovementStateConfig.YVelocity.y += _playerConfig.AirborneStateConfig.Gravity * Time.deltaTime;
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
