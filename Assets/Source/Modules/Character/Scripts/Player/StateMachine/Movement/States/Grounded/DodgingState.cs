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
        private bool _shouldKeepRotating;

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
            Data.MovementSpeedModifier = _dodgeStateConfig.SpeedModifier;
            
            base.Enter();
            
            PlayerView.StartDodging();

            #region ДЛЯ ЧЕГО ЭТО?
            AddForceTransitionFromStationaryState();
            _shouldKeepRotating = Data.MovementInput != Vector2.zero;
            UpdateConsecutiveDashes();
            #endregion
            
            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (Data.MovementInput == Vector2.zero)
            {
                return;
            }
            
            Dash();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();

            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
            }

            // StateSwitcher.SwitchState<SprintingState>(); //  ЭТО ИЗ ГЕНШИНА, НАМ НЕ НАДО ПЕРЕХОДИТЬ В СПРИНТ ПОСЛЕ КУВЫРКА
            // StateSwitcher.SwitchState<RunningState>();
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            
            StateSwitcher.SwitchState<IdlingState>();
        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            
            
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
                
                return;
            }
        }

        #endregion

        #region MAIN METHODS
        private void AddForceTransitionFromStationaryState()    //  СОСТОЯНИЕ ПРИ ПЕРЕХОДЕ ИЗ АНИМАЦИИ В АНИМАЦИЮ
        {
        }
        
        private void UpdateConsecutiveDashes()  //  ЛОГИКА ОТКЛЮЧЕНИЯ ВОЗМОЖНОСТИ НАЖИМАТЬ НА ПЕРЕКАТ
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
        
        private bool IsConsecutive()
        {
            return Time.time < _startTime + _dodgeStateConfig.TimeToBeConsideredConsecutive;
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
        
        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);
            
            _shouldKeepRotating = true;
        }
        #endregion
        
        #region INPUT METHODS
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            // StateSwitcher.SwitchState<RunningState>();  //  RUNNING(?)
        }

        protected override void OnDodgeStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
        
        private void Dash()
        {
            //  ЛОГИКА КУВЫРКА
        }
    }
}