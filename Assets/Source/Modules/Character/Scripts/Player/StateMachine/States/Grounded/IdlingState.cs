using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class IdlingState : GroundedState
    {
        public IdlingState(
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
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartIdling();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopIdling();
        }

        public override void Update()
        {
            base.Update();

            if (IsWalking())
                StateSwitcher.SwitchState<WalkingState>();
            else if (IsRunning())
                StateSwitcher.SwitchState<RunningState>();
            // else if (IsDodging())
            //     StateSwitcher.SwitchState<DodgingState>();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}