using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class DodgingState : GroundedState
    {
        private DodgeStateConfig _dodgeStateConfig;
        private MovementStateConfig _movementStateConfig;
        
        private float _startTime;
        private int _consecutiveDashedUsed;

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
            {
                _dodgeStateConfig = playerInputHandler.PlayerConfig.DodgeStateConfig;
                _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
            }
         
        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = _dodgeStateConfig.SpeedModifier;
            
            base.Enter();
            
            Dodge();
            
            PlayerView.StartDodging();

            #region ОТСЧЁТ ВРЕМЕНИ
            _startTime = Time.time;
            #endregion
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            _movementStateConfig.shouldDodge = false;
            
            StateSwitcher.SwitchState<IdlingState>();
        }
        #endregion

        #region REUSABLE METHODS
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Movement.performed -= OnMovementPerformed;
        }
        #endregion
        
        #region INPUT METHODS
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        #endregion

        protected override void OnDodgeStarted(InputAction.CallbackContext context)
        {
            base.OnDodgeStarted(context);
        }
    }
}