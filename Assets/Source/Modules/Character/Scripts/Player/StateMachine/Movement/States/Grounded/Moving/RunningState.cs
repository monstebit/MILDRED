using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class RunningState : MovingState
    {
        private float _startTime;
        private bool _keepSprinting;
        
        private RunningStateConfig _runningStateConfig;
        private SprintingStateConfig _sprintingStateConfig;
        
        public RunningState(
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
        {
            _runningStateConfig = playerInputHandler.PlayerConfig.RunningStateConfig;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _runningStateConfig.SpeedModifier;

            _startTime = Time.time;
            
            PlayerView.StartRunning();
        }

        public override void Exit()
        {
            base.Exit();

            _keepSprinting = false;
            
            PlayerView.StopRunning();
        }

        public override void Update()
        {
            base.Update();

            if (!Data.ShouldWalk)
            {
                return;
            }

            if (Time.time < _startTime + _sprintingStateConfig.RunToWalkTime)
            {
                return;
            }

            StopRunning();
        }
        #endregion

        #region MAIN METHODS
        private void StopRunning()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
                
                return;
            }
            
            StateSwitcher.SwitchState<WalkingState>();
        }
        #endregion
        
        #region INPUT METHODS
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            
            StateSwitcher.SwitchState<WalkingState>();
        }
        #endregion
    }
}