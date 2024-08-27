using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class FallingState : AirborneState
    {
        private readonly GroundChecker _groundChecker;
        private readonly MovementStateConfig _movementStateConfig;
        
        private readonly PlayerCameraMovement _playerCameraMovement;
        private readonly PlayerInputHandler _playerInputHandler;

        public FallingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher, playerInputHandler,
            characterNetworkManager,
            playerCameraMovement,
            data)
        {
            _groundChecker = playerInputHandler.GroundChecker;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;

            _playerInputHandler = playerInputHandler;
            _playerCameraMovement = playerCameraMovement;
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartFalling();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopFalling();
        }

        public override void Update()
        {
            base.Update();
            
            if (_groundChecker.isTouches)
            {
                _movementStateConfig.YVelocity.y = -2f;

                if (Data.MovementInput == Vector2.zero)
                {
                    StateSwitcher.SwitchState<IdlingState>();
                    
                    return;
                }
                
                if (_movementStateConfig.ShouldSprint)
                {
                    StateSwitcher.SwitchState<SprintingState>();
                
                    return;
                }
            
                if (_movementStateConfig.ShouldWalk)
                {
                    StateSwitcher.SwitchState<WalkingState>();
                
                    return;
                }
            
                StateSwitcher.SwitchState<RunningState>();
            }
        }
        
        protected override void ResetSprintState()
        {
            base.ResetSprintState();
        }
        
        protected override void ResetPerformingAction()
        {
            base.ResetPerformingAction();
        }
    }
}