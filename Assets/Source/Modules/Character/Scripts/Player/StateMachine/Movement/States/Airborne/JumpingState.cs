using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class JumpingState : AirborneState
    {
        private PlayerConfig _playerConfig;
        private JumpingStateConfig _jumpingStateConfig;
        private PlayerCompositionRoot _playerCompositionRoot;
        private readonly GroundChecker _groundChecker;
        private Vector3 jumpDirection;

        public JumpingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _groundChecker = playerCompositionRoot.GroundChecker;
            _playerConfig = playerCompositionRoot.PlayerConfig;
            _jumpingStateConfig = _playerConfig.AirborneStateConfig.JumpingStateConfig;
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartJumping();
            
            // Data.MovementSpeedModifier = _jumpingStateConfig.SpeedModifier;
            Data.MovementSpeedModifier = 0f;
            
            _jumpingStateConfig.IsJumping = true;
            Keyframe LastFrame = _jumpingStateConfig.JumpCurve[_jumpingStateConfig.JumpCurve.length - 1];
            _jumpingStateConfig.JumpTimer = LastFrame.time;
        }

        public override void Exit()
        {
            base.Exit();
            
            _jumpingStateConfig.Timer = 0;
            _jumpingStateConfig.IsJumping = false;
            
            if (InAnimationTransition())
            {
                return;
            }
            
            PlayerView.StopJumping();
        }
        
        public override void Update()
        {
            base.Update();
            
            HandleJump();
        }
        
        private void HandleJump()
        {
            if (_jumpingStateConfig.IsJumping)
            {
                _jumpingStateConfig.Timer += Time.deltaTime;

                // if (_jumpingStateConfig.Timer > 0.5f)
                if (_jumpingStateConfig.Timer > _jumpingStateConfig.JumpTimer)
                {
                    if (_groundChecker.isTouches)
                    {
                        StateSwitcher.SwitchState<LightLandingState>();
                     
                        // if (Data.MovementInput == Vector2.zero)
                        // {
                        //     StateSwitcher.SwitchState<IdlingState>();
                        // }
                        #region WITHOUT LANDING
                        // OnMove();
                        #endregion
                    }
                    
                    // StateSwitcher.SwitchState<FallingState>();
                }

                if (_jumpingStateConfig.Timer < _jumpingStateConfig.JumpTimer)
                {
                    // Обновляем вертикальную скорость (вверх-вниз)
                    _playerConfig.MovementStateConfig.YVelocity.y = 
                        _jumpingStateConfig.JumpCurve.Evaluate(_jumpingStateConfig.Timer);
                }
            }
            else
            {
                Exit();
            }
        }
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}