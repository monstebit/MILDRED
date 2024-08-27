using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly MovementStateConfig _movementStateConfig;
        private readonly AirborneStateConfig _airborneStateConfig;
        private float _gravity = -9.81f;

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

            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
            _playerInputHandler = playerInputHandler;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartAirborne();

            ResetSprintState();
            ResetPerformingAction();
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
        
        public void AirbornMove()
        {
            _movementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerInputHandler.CharacterController.Move(
                _movementStateConfig._movementDirection * Data.BaseSpeed * Time.deltaTime);
        }
    }
}