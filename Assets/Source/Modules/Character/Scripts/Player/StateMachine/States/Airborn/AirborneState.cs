using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Airborn
{
    public abstract class AirbornState : MovementState
    {
        private readonly AirbornStateConfig _airbornStateConfig;
        private readonly CharacterController _characterController;
        
        public AirbornState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement, StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            cameraMovement,
            data)
            => _airbornStateConfig = playerInputHandler.PlayerConfig.AirbornStateConfig;
            // {
            //     _characterController = playerInputHandler.CharacterController;
            // }

        public override void Enter()
        {
            base.Enter();

            Data.Speed = _airbornStateConfig.Speed;
            
            PlayerView.StartAirborne();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
        }

        public override void Update()
        {
            base.Update();

            Data.YVelocity -= _airbornStateConfig.BaseGravity * Time.deltaTime;
        }
    }
}