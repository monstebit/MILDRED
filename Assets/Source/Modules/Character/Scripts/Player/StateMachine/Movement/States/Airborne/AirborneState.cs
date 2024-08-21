using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly AirborneStateConfig _airborneStateConfig;
        private readonly SprintingStateConfig _sprintingStateConfig;
        private readonly MovementStateConfig _movementStateConfig;
        
        private PlayerCameraMovement _playerCameraMovement;
        private PlayerInputHandler _playerInputHandler;

        public AirborneState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement, StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerCameraMovement,
            data)
        {
            _airborneStateConfig = playerInputHandler.PlayerConfig.AirborneStateConfig;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
            
            _playerInputHandler = playerInputHandler;
            _playerCameraMovement = playerCameraMovement;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartAirborne();

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
        }

        public override void Update()
        {
            base.Update();
            
            ApplyGravity();

            AirbornMove();
        }

        private void ApplyGravity()
        {
            float gravity = -9.81f;
            _movementStateConfig.YVelocity.y += gravity * Time.deltaTime;
        }
        
        protected virtual void ResetSprintState()
        {
            _movementStateConfig.ShouldSprint = false;
        }
        
        
        
        public void AirbornMove()
        {
            _movementDirection = GetMovementInputDirection();
            
            _playerInputHandler.CharacterController.Move(
                _movementDirection * Data.BaseSpeed * Time.deltaTime);
        }
        // public virtual void AirbornMove()
        // {
        //     // _movementDirection = GetMovementInputDirection();
        //     Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
        //     Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
        //     Vector3 movementDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;
        //     movementDirection.y = _movementStateConfig.YVelocity.y;
        //     
        //     movementDirection.Normalize();
        //     // float movementSpeed = GetMovementSpeed();
        //     
        //     _playerInputHandler.CharacterController.Move(
        //         _movementDirection * Data.BaseSpeed * Time.deltaTime);
        // }
    }
}