using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public class WalkingState : MovementState
    {
        private WalkingStateConfig _walkingStateConfig;

        public WalkingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager,
            StateMachineData data) : base(stateSwitcher, playerInputHandler, characterNetworkManager, data)
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

            if (!IsPlayerWalking())
            {
                StateSwitcher.SwitchState<IdlingState>();
            }
            else if (IsPlayerSprinting())
            {
                StateSwitcher.SwitchState<SprintingState>();
            }
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}