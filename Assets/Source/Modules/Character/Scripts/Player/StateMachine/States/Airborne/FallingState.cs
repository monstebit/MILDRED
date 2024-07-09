using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Airborne
{
    public class FallingState : AirborneState
    {
        private readonly GroundChecker _groundChecker;

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
            => _groundChecker = playerInputHandler.GroundChecker;

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
                Data.YVelocity = 0;
                
                if (IsIdling())
                {
                    StateSwitcher.SwitchState<IdlingState>();
                }
                else if (IsWalking())
                {
                    StateSwitcher.SwitchState<WalkingState>();
                }
                else if (IsRunning())
                {
                    StateSwitcher.SwitchState<RunningState>();
                }
            }
        }
    }
}