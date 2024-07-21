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
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            Data.YVelocity = _jumpingStateConfig.StartYVelocity;
            
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
            
            if (Data.YVelocity < 0)
                StateSwitcher.SwitchState<FallingState>();
        }
        #endregion

        #region ЭТОТ МЕТОД РАБОТАЕТ НЕЗАВИСИМО ОТ base.ResetSprintState В ТЕЛЕ МЕТОДА!
        protected override void ResetSprintState()
        {
        }
        #endregion
    }
}