using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class IdlingState : GroundedState
    {
        public IdlingState(
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

            if (IsPlayerIdling())
            {
                return;
            }
    
            if (IsPlayerWalking())
            {
                StateSwitcher.SwitchState<WalkingState>();
            }
            else if (IsPlayerRunning())
            {
                StateSwitcher.SwitchState<RunningState>();
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}