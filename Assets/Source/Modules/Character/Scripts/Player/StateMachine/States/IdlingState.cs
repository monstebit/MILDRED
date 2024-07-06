using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public class IdlingState : MovementState
    {
        public IdlingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager, 
            StateMachineData data) : base(stateSwitcher, playerInputHandler, characterNetworkManager, data)
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