using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class WalkingState : GroundedState
    {
        private WalkingStateConfig _walkingStateConfig;

        public WalkingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher, 
            playerInputHandler, 
            characterNetworkManager, 
            playerCameraMovement, 
            data)
            => _walkingStateConfig = playerInputHandler.PlayerConfig.WalkingStateConfig;
        
        public override void Enter()
        {
            base.Enter();
            
            Data.Speed = _walkingStateConfig.WalkingSpeed;
            
            PlayerView.StartWalking();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopWalking();
        }

        public override void Update()
        {
            base.Update();
            
            if (IsIdling())
                StateSwitcher.SwitchState<IdlingState>();
            else if (IsRunning())
                StateSwitcher.SwitchState<RunningState>();
            else if (IsDodging())
                StateSwitcher.SwitchState<DodgingState>();
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}