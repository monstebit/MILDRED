using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        
        public GroundedState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartGrounded();
            
            UpdateShouldSprintState();
        }
        
        private void UpdateShouldSprintState()
        {
            if (!_playerConfig.MovementStateConfig.ShouldSprint)
            {
                return;
            }
            
            if (Data.MovementInput != Vector2.zero)
            {
                return;
            }
            
            _playerConfig.MovementStateConfig.ShouldSprint = false;
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopGrounded();
        }

        public override void Update()
        {
            base.Update();
            
            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                StateSwitcher.SwitchState<FallingState>();
            }
        }
        #endregion
        
        protected void OnMove()
        {
            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                
                return;
            }
            
            if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
        
        #region REUSABLE METHODS
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
        #endregion
        
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
            {
                return;
            }
            
            StateSwitcher.SwitchState<JumpingState>();
        }
    }
}