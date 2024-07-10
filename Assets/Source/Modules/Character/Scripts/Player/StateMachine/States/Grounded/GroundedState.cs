using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private readonly GroundChecker _groundChecker;
        private readonly MovementStateConfig _movementStateConfig;  //  TEST

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
            // => _groundChecker = playerInputHandler.GroundChecker;
        {
            _groundChecker = playerInputHandler.GroundChecker;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig; //  TEST
        }

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

            if (_groundChecker.isTouches == false)
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        
            PlayerControls.PlayerMovement.Jump.started += OnJumpButtonPressed;
            PlayerControls.PlayerMovement.Dodge.started += OnDodgeButtonPressed;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Jump.started -= OnJumpButtonPressed;
            PlayerControls.PlayerMovement.Dodge.started -= OnDodgeButtonPressed;
        }

        private void OnJumpButtonPressed(InputAction.CallbackContext obj)
        {
            StateSwitcher.SwitchState<JumpingState>();
        }
        
        private void OnDodgeButtonPressed(InputAction.CallbackContext obj)
        {
            if (IsIdling()) //  МЫ НЕ МОЖЕМ СДЕЛАТЬ КУВЫРОК ИЗ СОСТОЯНИЯ IDLE
                return;
            
            if (IsDodging())
                return;
            
            Debug.Log("ВЫПОЛНЯЮ ДЕЙСТВИЕ ПЕРЕКАТА");
            StateSwitcher.SwitchState<DodgingState>();
        }
    }
}