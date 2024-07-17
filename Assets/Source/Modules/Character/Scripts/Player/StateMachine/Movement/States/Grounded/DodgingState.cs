using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class DodgingState : GroundedState
    {
        private DodgeStateConfig _dodgeStateConfig;
        private PlayerInputHandler _playerInputHandler;
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
                _playerInputHandler = playerInputHandler;
            }
         
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _dodgeStateConfig.SpeedModifier;
            
            AddForceTransitionFromStationaryState();
            UpdateConsecutiveDashes();
            _startTime = Time.time;
            // PlayerView.StartDodging();
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();

            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
            }

            StateSwitcher.SwitchState<SprintingState>();
        }

        #endregion

        #region MAIN METHODS
        private void AddForceTransitionFromStationaryState()
        {
            if (Data.MovementInput != Vector2.zero)
            {
                return;
            }

            Vector3 playerRotationDirection = PlayerView.transform.forward;
            playerRotationDirection.y = 0f;
            //чето с ригидбади было
        }
        
        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
            {
                _consecutiveDashedUsed = 0;
            }

            ++_consecutiveDashedUsed;

            if (_consecutiveDashedUsed == _dodgeStateConfig.ConsecutiveDashesLimitAmount)
            {
                _consecutiveDashedUsed = 0;

                _playerInputHandler.DisableActionFor(
                    PlayerControls.PlayerMovement.Dodge,
                    _dodgeStateConfig.DashLimitReachedCooldown);
            }
        }
        #endregion

        private bool IsConsecutive()
        {
            return Time.time < _startTime + _dodgeStateConfig.TimeToBeConsideredConsecutive;
        }
        
        #region INPUT METHODS

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }

        protected override void OnDodgeStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}