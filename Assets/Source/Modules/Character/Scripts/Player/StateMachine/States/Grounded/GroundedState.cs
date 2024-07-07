using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Airborn;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private readonly GroundChecker _groundChecker;

        public GroundedState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            cameraMovement,
            data) 
            => _groundChecker = playerInputHandler.GroundChecker;

        public override void Update()
        {
            base.Update();

            //  ОБРУБАЕТ УПРАВЛЕНИЕ
            if (_groundChecker.isTouches == false)
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        
            PlayerControls.PlayerMovement.Jump.started += OnJumpButtonPressed;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Jump.started -= OnJumpButtonPressed;
        }

        // private void OnJumpButtonPressed(InputAction.CallbackContext obj) => StateSwitcher.SwitchState<JumpingState>();
        private void OnJumpButtonPressed(InputAction.CallbackContext obj)
        {
            StateSwitcher.SwitchState<JumpingState>();
        }
    }
}