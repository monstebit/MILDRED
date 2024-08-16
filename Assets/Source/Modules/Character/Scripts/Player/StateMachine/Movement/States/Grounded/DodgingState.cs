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
        private PlayerConfig _playerConfig;
        
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
            
            PlayerView.StartDodging();

            _movementStateConfig.IsPerformingAction = true;
            
            _dodgeStateConfig._startTime = Time.time;
            
            StartDodge();
        }

        public override void Update()
        {
            base.Update();
            
            PerformDodge();
                
            if (_movementStateConfig.IsPerformingAction == false)
            {
                if (Data.MovementInput == Vector2.zero)
                {
                    StateSwitcher.SwitchState<IdlingState>();
                    
                    return;
                }

                OnMove();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
            
            _movementStateConfig.IsPerformingAction = false;
        }
        #endregion

        public override void OnAnimationExitEvent() //  ТРИГГЕР ЗАВЕРШЕНИЯ АНИМАЦИИ
        {
            base.OnAnimationExitEvent();
        }

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
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
    }
}