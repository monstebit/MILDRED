using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly PlayerConfig _playerConfig;
        
        protected AirborneState(
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
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
            
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
        
        private void ResetPerformingAction()
        {
            // _playerConfig.MovementStateConfig.IsPerformingStaticAction = false;
        }
        
        private void AirborneMove()  //  HORIZONTAL MOVEMENT IN AIR
        {
            _playerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                _playerConfig.MovementStateConfig._movementDirection * (Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime));
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }

        protected override void OnSprintCanceled(InputAction.CallbackContext context)
        {
            if (PlayerConfig.MovementStateConfig._isButtonHeld == false)
            {
                return;
            }

            PlayerConfig.MovementStateConfig._isButtonHeld = false;

            if (PlayerConfig.MovementStateConfig.ShouldSprint)
            {
                PlayerConfig.MovementStateConfig.ShouldSprint = false;
            }
        }
    }
}