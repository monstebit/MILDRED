using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private readonly GroundChecker _groundChecker;

        public GroundedState(
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
            _groundChecker = playerInputHandler.GroundChecker;
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartGrounded();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopGrounded();
        }

        public override void Update()
        {
            base.Update();
        }
        #endregion
        
        #region REUSABLE METHODS
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerControls.PlayerMovement.Movement.canceled += OnMovementCanceled;
            PlayerControls.PlayerMovement.Dodge.started += OnDodgeStarted;
            PlayerControls.PlayerMovement.Jump.started += OnJumpStarted;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Movement.canceled -= OnMovementCanceled;
            PlayerControls.PlayerMovement.Dodge.started -= OnDodgeStarted;
            PlayerControls.PlayerMovement.Jump.started -= OnJumpStarted;
        }
        
        protected void OnMove()
        {
            if (Data.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
        #endregion
        
        #region INPUT METHODS
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            // Debug.Log("Вышел из состояния ходьбы");
            StateSwitcher.SwitchState<IdlingState>();
        }
        
        protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<DodgingState>();
        }
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<JumpingState>();
        }
        #endregion
    }
}