using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;

        protected GroundedState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            // PlayerView.StartGrounded();
            PlayerView.UpdateState("IsGrounded", true);
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "IsGrounded", true);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            // PlayerView.StopGrounded();
            PlayerView.UpdateState("IsGrounded", false);
            // _playerCompositionRoot.PlayerNetworkSynchronizer.NotifyTheServerOfActionAnimationServerRpc(
            //     NetworkManager.Singleton.LocalClientId, "IsGrounded", false);
        }

        public override void Update()
        {
            base.Update();

            CheckGroundAndSwitchToFalling();
        }
        
        protected virtual void CheckGroundAndSwitchToFalling()
        {
            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                if (InAnimationTransition()) return;

                StateSwitcher.SwitchState<FallingState>();
            }
        }
        
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            
            PlayerControls.Player.Jump.performed += OnJumpStarted;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.Player.Jump.performed -= OnJumpStarted;
        }
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<JumpingState>();
        }
    }
}