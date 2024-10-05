using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class RunningState : MovingState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        private float _startTime;
        
        public RunningState(
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
            Data.MovementSpeedModifier = _playerConfig.RunningStateConfig.SpeedModifier;
            Data.JumpModifier = 1f;
            
            base.Enter();

            PlayerView.StartRunning();
            
            _startTime = Time.time;
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopRunning();
        }
        
        public override void Update()
        {
            base.Update();

            // if (Time.time < _startTime + _playerConfig.SprintingStateConfig.RunToWalkTime) return;
            OnMove();
        }
        
        protected override void OnMove()
        {
            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                return;
            }
            
            if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
            }
        }
        
        //  PC CONTROL
        protected override void OnWalkTogglePerformed(InputAction.CallbackContext context)
        {
            base.OnWalkTogglePerformed(context);
            
            StateSwitcher.SwitchState<WalkingState>();
        }
        
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
        }
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}