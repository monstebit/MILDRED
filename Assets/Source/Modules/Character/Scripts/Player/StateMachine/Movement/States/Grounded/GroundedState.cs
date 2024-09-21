using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public abstract class GroundedState : MovementState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly PlayerConfig _playerConfig;

        protected GroundedState(
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
        
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartGrounded();
            
            UpdateShouldSprintState();
        }
        
        private void UpdateShouldSprintState()
        {
            if (_playerConfig.MovementStateConfig.ShouldSprint == false)
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
                //  DO NOT GO OUT OF ACTION WHEN YOU FALL
                if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
                {
                    return; 
                }
                
                if (InAnimationTransition())
                {
                    return;
                }
                
                StateSwitcher.SwitchState<FallingState>();
            }
        }
        
        protected virtual void OnMove()
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
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}