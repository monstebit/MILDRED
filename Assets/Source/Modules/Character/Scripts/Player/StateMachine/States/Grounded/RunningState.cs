using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class RunningState : GroundedState
    {
        private RunningStateConfig _runningStateConfig;

        public RunningState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement,
            StateMachineData data) : base(
            stateSwitcher, 
            playerInputHandler, 
            characterNetworkManager, 
            cameraMovement, 
            data)
            => _runningStateConfig = playerInputHandler.PlayerConfig.RunningStateConfig;
        
        public override void Enter()
        {
            base.Enter();

            Data.Speed = _runningStateConfig.RunningSpeed;
            
            PlayerView.StartRunning();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopRunning();
        }

        public override void Update()
        {
            base.Update();
    
            if (IsPlayerIdling())
            {
                StateSwitcher.SwitchState<IdlingState>();
            }
            else if (IsPlayerWalking())
            {
                StateSwitcher.SwitchState<WalkingState>();
            }
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}