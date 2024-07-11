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

            _movementStateConfig.isDodging = true;
            PlayerView.ResetDodgeEndTrigger();

            PlayerView.StartDodging();
        }

        public override void Exit()
        {
            base.Exit();

            _movementStateConfig.isDodging = false;
            
            PlayerView.StopDodging();
        }

        public override void Update()
        {
            base.Update();
            
            if (PlayerView.DodgeEnds)
            {
                PlayerView.ResetDodgeEndTrigger();  // Сброс состояния события

                if (IsRunning())
                {
                    StateSwitcher.SwitchState<RunningState>();
                }
                else if (IsWalking())
                {
                    StateSwitcher.SwitchState<WalkingState>(); 
                }
                else
                {
                    StateSwitcher.SwitchState<IdlingState>();
                }
            }
        }
        
        // public override void Update()
        // {
        //     base.Update();
        //     
        //     if (!IsDodging())
        //     {
        //         if (IsRunning())
        //         {
        //             StateSwitcher.SwitchState<RunningState>();
        //         }
        //         else if (IsWalking())
        //         {
        //             StateSwitcher.SwitchState<WalkingState>(); 
        //         }
        //         else
        //         {
        //             StateSwitcher.SwitchState<IdlingState>();
        //         }
        //     }
        // }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}
