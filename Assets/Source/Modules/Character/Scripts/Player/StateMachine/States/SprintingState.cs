using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public class SprintingState : MovementState
    {
        private SprintingStateConfig _sprintingStateConfig;

        public SprintingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            StateMachineData data) : base(stateSwitcher, playerInputHandler, characterNetworkManager, data)
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