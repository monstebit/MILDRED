using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Airborn
{
    public class JumpingState : AirbornState
    {
        private readonly JumpingStateConfig _jumpingStateConfig;

        public JumpingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement,
            StateMachineData data) : base(
            stateSwitcher, playerInputHandler,
            characterNetworkManager,
            cameraMovement,
            data)
            => _jumpingStateConfig = playerInputHandler.PlayerConfig.AirbornStateConfig.JumpingStateConfig;

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}