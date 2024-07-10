using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class DodgingState : GroundedState
    {
        private bool isExiting = false;
        private float exitTimer = 0f;
        private const float delay = 2f;

        // Вызывается для начала процесса выхода
        public void StartExitProcess()
        {
            isExiting = true;
            exitTimer = 0f;
        }
        
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
            
            // _movementStateConfig.IsPerformingAction = true; //  TEST
            // _movementStateConfig.isDodging = true; //  TEST
            // PlayerView.StartPerformingAction();
            StartExitProcess();
            _movementStateConfig.isDodging = true;
        }

        public override void Exit()
        {
            base.Exit();
        
            PlayerView.StopDodging();
            // PlayerView.StopPerformingAction();
            _movementStateConfig.isDodging = false;
        }

        public override void Update()
        {
            base.Update();

            if (isExiting)  //  TEST
            {
                exitTimer += Time.deltaTime;
                if (exitTimer >= delay)
                {
                    Exit();
                    isExiting = false;
                }
            }
            
            if (IsIdling())
                return;
            
            if (!IsDodging())
            {
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
            // if (_movementStateConfig.IsPerformingAction && _movementStateConfig.isDodging)
            // {
            //     StateSwitcher.SwitchState<DodgingState>();
            // }
            // else if (IsIdling())
            // {
            //     StateSwitcher.SwitchState<IdlingState>();
            // }
            // else if (IsWalking())
            // {
            //     StateSwitcher.SwitchState<WalkingState>();
            // }
            // else if (IsRunning())
            // {
            //     StateSwitcher.SwitchState<RunningState>();
            // }
        }
        
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}
