using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Airborne
{
    public class JumpingState : AirborneState
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
            => _jumpingStateConfig = playerInputHandler.PlayerConfig.AirborneStateConfig.JumpingStateConfig;

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
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }
    }
}