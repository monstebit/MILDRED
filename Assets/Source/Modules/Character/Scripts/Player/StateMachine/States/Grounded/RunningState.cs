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
            PlayerCameraMovement playerPlayerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher, 
            playerInputHandler, 
            characterNetworkManager, 
            playerPlayerCameraMovement, 
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

            if (IsIdling())
                StateSwitcher.SwitchState<IdlingState>();
            else if (IsWalking())
                StateSwitcher.SwitchState<WalkingState>();
            else if (IsRunning())
                StateSwitcher.SwitchState<RunningState>();
            else if (IsDodging())
                StateSwitcher.SwitchState<DodgingState>();
            // if (IsIdling())
            // {
            //     StateSwitcher.SwitchState<IdlingState>();
            // }
            // else if (IsWalking())
            // {
            //     StateSwitcher.SwitchState<WalkingState>();
            // }
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}