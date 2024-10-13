using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class JumpingState : AirborneState
    {
        private readonly GroundChecker _groundChecker;
        private PlayerConfig _playerConfig;
        private JumpingStateConfig _jumpingStateConfig;

        public JumpingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _groundChecker = playerCompositionRoot.GroundChecker;
            _playerConfig = playerCompositionRoot.PlayerConfig;
            _jumpingStateConfig = _playerConfig.AirborneStateConfig.JumpingStateConfig;
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerView.UpdateState("IsJumping", true);
            
            Data.MovementSpeedModifier = 0f;
            
            _jumpingStateConfig.IsJumping = true;
            Keyframe lastFrame = _jumpingStateConfig.JumpCurve[_jumpingStateConfig.JumpCurve.length - 1];
            _jumpingStateConfig.JumpTimer = lastFrame.time;
        }

        public override void Exit()
        {
            base.Exit();
            
            if (InAnimationTransition())
            {
                return;
            }
            
            _jumpingStateConfig.Timer = 0;
            _jumpingStateConfig.IsJumping = false;
            
            PlayerView.UpdateState("IsJumping", false);
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

                if (_jumpingStateConfig.Timer > _jumpingStateConfig.JumpTimer)
                {
                    if (_groundChecker.isTouches)
                    {
                        StateSwitcher.SwitchState<LightLandingState>();
                    }
                }

                if (_jumpingStateConfig.Timer < _jumpingStateConfig.JumpTimer)
                {
                    _playerConfig.MovementStateConfig.YVelocity.y = 
                        _jumpingStateConfig.JumpCurve.Evaluate(_jumpingStateConfig.Timer);
                }
            }
            else
            {
                Exit();
            }
        }
    }
}