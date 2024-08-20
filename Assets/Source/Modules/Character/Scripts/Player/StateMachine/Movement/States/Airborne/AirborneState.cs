using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly AirborneStateConfig _airborneStateConfig;
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly SprintingStateConfig _sprintingStateConfig;
        private readonly MovementStateConfig _movementStateConfig;

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

            // HandleVerticalMovement();
            
            // Data.YVelocity.y -= _airborneStateConfig.BaseGravity * Time.deltaTime;
            // Data.YVelocity -= _airborneStateConfig.BaseGravity * Time.deltaTime;
            ApplyGravity();
            
            // if (_movementStateConfig.YVelocity.y < 0)
            // {
            //     StateSwitcher.SwitchState<FallingState>();
            // }
        }

        private void ApplyGravity()
        {
            float gravity = -9.81f;
            _movementStateConfig.YVelocity.y += gravity * Time.deltaTime;
        }
        
        public void HandleVerticalMovement()
        {
            _playerInputHandler.CharacterController.Move(
                _movementDirection * 5 * Time.deltaTime);
        }
        
        protected virtual void ResetSprintState()
        {
            _movementStateConfig.ShouldSprint = false;
        }
    }
}