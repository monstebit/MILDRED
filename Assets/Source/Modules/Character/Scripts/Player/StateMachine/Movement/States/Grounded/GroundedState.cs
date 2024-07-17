using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
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
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Movement.canceled -= OnMovementCanceled;
            PlayerControls.PlayerMovement.Dodge.started -= OnDodgeStarted;
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
        #endregion

        // public override void Enter()
        // {
        //     base.Enter();
        //     
        //     PlayerView.StartGrounded();
        // }
        //
        // public override void Exit()
        // {
        //     base.Exit();
        //     
        //     PlayerView.StopGrounded();
        // }
        //
        // public override void Update()
        // {
        //     base.Update();
        //
        //     if (_groundChecker.isTouches == false)
        //     {
        //         StateSwitcher.SwitchState<FallingState>();
        //     }
        // }
        //
        // protected override void AddInputActionsCallbacks()
        // {
        //     base.AddInputActionsCallbacks();
        //
        //     PlayerControls.PlayerMovement.Jump.started += OnJumpButtonPressed;
        //     PlayerControls.PlayerMovement.Dodge.started += OnDodgeButtonPressed;
        // }
        //
        // protected override void RemoveInputActionsCallbacks()
        // {
        //     base.RemoveInputActionsCallbacks();
        //     
        //     PlayerControls.PlayerMovement.Jump.started -= OnJumpButtonPressed;
        //     PlayerControls.PlayerMovement.Dodge.started -= OnDodgeButtonPressed;
        // }
        //
        // private void OnJumpButtonPressed(InputAction.CallbackContext obj)
        // {
        //     if (IsDodging())
        //         return;
        //     
        //     // Debug.Log("JUMP BUTTON PRESSED!");
        //     StateSwitcher.SwitchState<JumpingState>();
        // }
        //
        // private void OnDodgeButtonPressed(InputAction.CallbackContext obj)
        // {
        //     if (IsIdling()) //  МЫ НЕ МОЖЕМ СДЕЛАТЬ КУВЫРОК ИЗ СОСТОЯНИЯ IDLE
        //         return;
        //     
        //     if (IsDodging())
        //         return;
        //     
        //     // Debug.Log("DODGE BUTTON PRESSED!");
        //     StateSwitcher.SwitchState<DodgingState>();
        // }
    }
}