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

            UpdateShouldSprintState();  //  FOW WHAT?
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

            CheckGroundAndSwitchToFalling();
        }
        
        protected virtual void CheckGroundAndSwitchToFalling()
        {
            // Проверка, касаемся ли земли
            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                // Если идёт анимационный переход, выходим из метода
                if (InAnimationTransition())
                {
                    return;
                }

                // Переход в состояние падения
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
        
        protected virtual bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}