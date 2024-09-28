using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly PlayerConfig _playerConfig;

        private float _aiborneRotationSpeed;
        
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
        }

        public override void Exit()
        {
            base.Exit();
            
            _playerConfig.MovementStateConfig.YVelocity.x = 0;
            
            PlayerView.StopAirborne();

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
        
        protected override void Move()
        {
            // if (_playerCompositionRoot.GroundChecker.isTouches == false && Data.JumppeedModifier == 0f)
            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                PlayerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
                _playerCompositionRoot.CharacterController.Move(
                    // PlayerConfig.MovementStateConfig._movementDirection * (Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime));
                    PlayerConfig.MovementStateConfig._movementDirection * (_playerConfig.AirborneStateConfig.JumpingStateConfig.IdleJumpingSpeed * Time.deltaTime));
            }
        }

        protected override void Rotate()
        {
            _aiborneRotationSpeed = 1f; //  ON TESTING
            
            if (PlayerConfig.MovementStateConfig._movementDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(PlayerConfig.MovementStateConfig._movementDirection);

                Quaternion targetRotation = Quaternion.Slerp(
                    PlayerView.transform.rotation,
                    newRotation,
                    _aiborneRotationSpeed * Time.deltaTime);

                PlayerView.transform.rotation = targetRotation;
            }
        }
        
        private void ResetSprintState()
        {
            PlayerConfig.MovementStateConfig._timeButtonHeld = 0f;
            PlayerConfig.MovementStateConfig.ShouldSprint = false;
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
        // protected override void OnStaticActionCanceled(InputAction.CallbackContext context)
        // {
        //     if (PlayerConfig.MovementStateConfig._isButtonHeld == false)
        //     {
        //         return;
        //     }
        //
        //     PlayerConfig.MovementStateConfig._isButtonHeld = false;
        //
        //     if (PlayerConfig.MovementStateConfig.ShouldSprint)
        //     {
        //         PlayerConfig.MovementStateConfig.ShouldSprint = false;
        //     }
        // }
    }
}