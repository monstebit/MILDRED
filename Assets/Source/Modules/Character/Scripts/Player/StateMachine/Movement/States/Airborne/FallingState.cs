using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class FallingState : AirborneState
    {
        private readonly GroundChecker _groundChecker;
        private MovementStateConfig _movementStateConfig;

        public FallingState(
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
            _groundChecker = playerInputHandler.GroundChecker;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartFalling();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopFalling();
        }

        public override void Update()
        {
            base.Update();
            
            if (_groundChecker.isTouches)
            {
                _movementStateConfig.shouldAirborne = false;
                
                Data.YVelocity = 0;
                
                StateSwitcher.SwitchState<IdlingState>();
            }
        }

        #region ЭТОТ МЕТОД РАБОТАЕТ НЕЗАВИСИМО ОТ base.ResetSprintState В ТЕЛЕ МЕТОДА!
        protected override void ResetSprintState()
        {
        }
        #endregion
    }
}