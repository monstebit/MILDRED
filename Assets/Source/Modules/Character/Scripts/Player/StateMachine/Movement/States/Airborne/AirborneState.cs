using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly MovementStateConfig _movementStateConfig;
        private readonly AirborneStateConfig _airborneStateConfig;
        private float _gravity = -9.81f;

        public AirborneState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
            _playerCompositionRoot = playerCompositionRoot;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartAirborne();
            
            ResetPerformingAction();
            
            _movementStateConfig.IsAirborning = true;
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
            
            _movementStateConfig.IsAirborning = false;
        }

        public override void Update()
        {
            base.Update();
            
            ApplyGravity();

            AirborneMove();
        }

        private void ApplyGravity()
        {
            _movementStateConfig.YVelocity.y += _gravity * Time.deltaTime;
        }
        
        protected virtual void ResetSprintState()
        {
            _movementStateConfig.ShouldSprint = false;
        }
        
        protected virtual void ResetPerformingAction()
        {
            _movementStateConfig.IsPerformingAction = false;
        }
        
        public void AirborneMove()  //  ГОРИЗОНТАЛЬНОЕ ДВИЖЕНИЕ В ВОЗДУХЕ
        {
            _movementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                // _movementStateConfig._movementDirection * Data.MovementSpeedModifier * Time.deltaTime);
                _movementStateConfig._movementDirection * Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime);
        }
        
        //  TEST
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }
    }
}