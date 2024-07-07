using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class SprintingState : GroundedState
    {
        private SprintingStateConfig _sprintingStateConfig;

        public SprintingState(
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
            => _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        
        public override void Enter()
        {
            base.Enter();

            Data.Speed = _sprintingStateConfig.SprintingSpeed;
            
            PlayerView.StartSprinting();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopSprinting();
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