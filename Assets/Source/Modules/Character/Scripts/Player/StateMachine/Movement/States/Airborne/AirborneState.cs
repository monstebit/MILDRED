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
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartAirborne();

            ResetSprintState();
            ResetDodgeState();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
        }

        public override void Update()
        {
            base.Update();
            
            Data.YVelocity -= _airborneStateConfig.BaseGravity * Time.deltaTime;
        }

        #region RESET STATES
        protected virtual void ResetSprintState()
        {
            _sprintingStateConfig.ShouldSprint = false;
        }
        
        protected virtual void ResetDodgeState()
        {
            _movementStateConfig.shouldDodge = false;
        }
        #endregion
    }
}