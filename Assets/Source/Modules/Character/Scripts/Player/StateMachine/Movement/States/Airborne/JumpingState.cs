using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class JumpingState : AirborneState
    {
        private readonly JumpingStateConfig _jumpingStateConfig;
        
        private PlayerInputHandler _playerInputHandler;
        private PlayerCameraMovement _playerCameraMovement;
        private SprintingStateConfig _sprintingStateConfig;
        private MovementStateConfig _movementStateConfig;
        
        private Vector3 jumpDirection;

        public JumpingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher, playerInputHandler,
            characterNetworkManager,
            playerCameraMovement,
            data)
        {
            _jumpingStateConfig = playerInputHandler.PlayerConfig.AirborneStateConfig.JumpingStateConfig;
            _playerInputHandler = playerInputHandler;
            _playerCameraMovement = playerCameraMovement;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            ApplyJumpForce();
            
            PlayerView.StartJumping();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopJumping();
        }

        public override void Update()
        {
            base.Update();
            
            if (_movementStateConfig.YVelocity.y < 0)
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }
        #endregion
        
        private void ApplyJumpForce()
        {
            float jumpForce = 2f;
            float gravity = -9.81f;
            _movementStateConfig.YVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        
        protected override void ResetSprintState()
        {
            base.ResetSprintState();
        }
    }
}