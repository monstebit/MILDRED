using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        
        public AirborneState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartAirborne();
            
            ResetPerformingAction();
            _playerConfig.MovementStateConfig.IsAirborning = true;
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
            
            _playerConfig.MovementStateConfig.IsAirborning = false;
            
            //  ON TESTING
            _playerConfig.AirborneStateConfig.InAirTime = 0f;
        }

        public override void Update()
        {
            base.Update();
            
            ApplyGravity();
            AirborneMove();
        }

        private void ApplyGravity()
        {
            _playerConfig.MovementStateConfig.YVelocity.y += _playerConfig.AirborneStateConfig.Gravity * Time.deltaTime;
        }
        
        protected virtual void ResetSprintState()
        {
            _playerConfig.MovementStateConfig.ShouldSprint = false;
        }
        
        protected virtual void ResetPerformingAction()
        {
            _playerConfig.MovementStateConfig.IsPerformingStaticAction = false;
        }
        
        public void AirborneMove()  //  ГОРИЗОНТАЛЬНОЕ ДВИЖЕНИЕ В ВОЗДУХЕ
        {
            _playerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                _playerConfig.MovementStateConfig._movementDirection * Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime);
        }
        
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