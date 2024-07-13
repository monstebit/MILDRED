using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class DodgingState : GroundedState
    {
        private MovementStateConfig _movementStateConfig;
    
        public DodgingState(
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
            => _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
         
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartDodging();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
        }

        public override void Update()
        {
            base.Update();

            if (IsDodging()) 
                return;
            
            if (IsIdling())
                StateSwitcher.SwitchState<IdlingState>();
            else if (IsWalking())
                StateSwitcher.SwitchState<WalkingState>();
            else if (IsRunning())
                StateSwitcher.SwitchState<RunningState>();
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}