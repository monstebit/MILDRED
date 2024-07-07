using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

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
            
            Data.YVelocity = _jumpingStateConfig.StartYVelocity;
        }

        public override void Update()
        {
            base.Update();
            
            if (Data.YVelocity < 0)
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }
    }
}