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
        private MovementStateConfig _movementStateConfig;
        
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

            _dodgeStateConfig._startTime = Time.time;
            
            StartDodge();
        }

        public override void Update()
        {
            base.Update();
            
            if (_movementStateConfig.shouldDodge)
            {
                PerformDodge();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
        }
        #endregion

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            _movementStateConfig.shouldDodge = false;

            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
                return;
            }

            if (_movementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
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