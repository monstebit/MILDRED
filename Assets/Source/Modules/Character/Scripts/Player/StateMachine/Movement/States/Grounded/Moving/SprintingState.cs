using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        private SprintingStateConfig _sprintingStateConfig;

        private bool _keepSprinting;
        private float _startTime;
        
        public SprintingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager, 
            PlayerCameraMovement playerPlayerCameraMovement, 
            StateMachineData data) : base(stateSwitcher, 
            playerInputHandler, characterNetworkManager, 
            playerPlayerCameraMovement, 
            data)
        {
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _sprintingStateConfig.SpeedModifier;

            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (_keepSprinting)
            {
                return;
            }

            if (Time.time < _startTime + _sprintingStateConfig.SprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }
        #endregion

        #region MAIN METHODS
        private void StopSprinting()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
                
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
        #endregion
        
        #region REUSABLE METHODS

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerControls.PlayerMovement.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Sprint.performed -= OnSprintPerformed;
        }
        #endregion

        #region INPUT METHODS
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _keepSprinting = true;
        }
        #endregion
    }
}