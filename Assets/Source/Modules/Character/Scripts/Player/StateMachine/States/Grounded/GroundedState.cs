using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Airborne;
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
            PlayerCameraMovement playerPlayerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerPlayerCameraMovement,
            data) 
            => _groundChecker = playerInputHandler.GroundChecker;

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
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Jump.started -= OnJumpButtonPressed;
        }

        private void OnJumpButtonPressed(InputAction.CallbackContext obj)
        {
            StateSwitcher.SwitchState<JumpingState>();
        }
    }
}