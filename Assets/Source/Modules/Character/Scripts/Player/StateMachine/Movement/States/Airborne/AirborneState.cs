using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        private float _airborneRotationSpeed;
        
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
            
            PlayerView.UpdateState("IsAirborne", true);
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.UpdateState("IsAirborne", false);

            ResetSprintState();
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
        
        private void AirborneMove()
        {
            _playerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                PlayerView.transform.forward * (Data.JumpModifier * PlayerConfig.AirborneStateConfig.JumpingStateConfig.ForwardSpeed * Time.deltaTime));
        }
        
        private void ResetSprintState()
        {
            PlayerConfig.MovementStateConfig._timeButtonHeld = 0f;
            PlayerConfig.MovementStateConfig.ShouldSprint = false;
        }
        
        protected override void Move()
        {
            if (_playerCompositionRoot.GroundChecker.isTouches)
            {
                return;
            }
            
            PlayerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                PlayerConfig.MovementStateConfig._movementDirection * (_playerConfig.AirborneStateConfig.JumpingStateConfig.IdleJumpingSpeed * Time.deltaTime));
        }

        protected override void Rotate()
        {
            _airborneRotationSpeed = 1f;

            if (PlayerConfig.MovementStateConfig._movementDirection == Vector3.zero)
            {
                return;
            }
            
            Quaternion newRotation = Quaternion.LookRotation(PlayerConfig.MovementStateConfig._movementDirection);

            Quaternion targetRotation = Quaternion.Slerp(
                PlayerView.transform.rotation,
                newRotation,
                _airborneRotationSpeed * Time.deltaTime);

            PlayerView.transform.rotation = targetRotation;
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
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