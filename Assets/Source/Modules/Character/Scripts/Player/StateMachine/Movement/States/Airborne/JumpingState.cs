using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public class JumpingState : AirborneState
    {
        private PlayerConfig _playerConfig;
        private JumpingStateConfig _jumpingStateConfig;
        private Vector3 jumpDirection;

        public JumpingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerConfig = playerCompositionRoot.PlayerConfig;
            _jumpingStateConfig = _playerConfig.AirborneStateConfig.JumpingStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartJumping();

            _jumpingStateConfig.SpeedModifier = GetSpeedModifier();
            Data.MovementSpeedModifier = _jumpingStateConfig.SpeedModifier;
            _jumpingStateConfig.IsJumping = true;
            Keyframe LastFrame = _jumpingStateConfig.JumpCurve[_jumpingStateConfig.JumpCurve.length - 1];
            _jumpingStateConfig.JumpTimer = LastFrame.time;
        }

        public override void Update()
        {
            base.Update();
            
            if (_jumpingStateConfig.IsJumping)
            {
                _jumpingStateConfig.Timer += Time.deltaTime;
            
                if (_jumpingStateConfig.Timer > 0.5f)
                {
                    StateSwitcher.SwitchState<FallingState>();
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
                // _jumpingStateConfig.Timer = 0;
                // _jumpingStateConfig.IsJumping = false;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopJumping();
            
            _jumpingStateConfig.Timer = 0;
            _jumpingStateConfig.IsJumping = false;
        }
        #endregion
        
        private float GetSpeedModifier()
        {
            float speedModifier = 0.75f;

            if (Data.MovementInput == Vector2.zero)
            {
                speedModifier = 0.25f;
            }
            
            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                speedModifier = 1;
            }
            else if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                speedModifier = 0.5f;
            }

            return speedModifier;
        }
        
        private void ApplyJumpForce()   //  =ПРИГОДИТСЯ=
        {
            _playerConfig.MovementStateConfig.YVelocity.y = 
                Mathf.Sqrt(_jumpingStateConfig.JumpForce * -2f * _playerConfig.AirborneStateConfig.Gravity);
        }
    }
}