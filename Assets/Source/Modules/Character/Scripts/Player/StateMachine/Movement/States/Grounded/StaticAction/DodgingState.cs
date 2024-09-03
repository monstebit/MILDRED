using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class DodgingState : StaticActionState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        
        public DodgingState(
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
            
            PlayerView.StartDodging();
            
            Keyframe LastFrame = _playerConfig.DodgeStateConfig.DodgeCurve[_playerConfig.DodgeStateConfig.DodgeCurve.length - 1];
            _playerConfig.DodgeStateConfig.DodgeTimer = LastFrame.time;
            _playerConfig.DodgeStateConfig.LastDodgeDirection = PlayerView.transform.forward;
            _playerConfig.DodgeStateConfig.LastDodgeDirection.y = 0;
            _playerConfig.DodgeStateConfig.LastDodgeDirection.Normalize();
        }

        public override void Update()
        {
            base.Update();
            
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
            {
                _playerConfig.DodgeStateConfig.Timer += Time.deltaTime;

                if (_playerConfig.DodgeStateConfig.Timer < _playerConfig.DodgeStateConfig.DodgeTimer)
                {
                    float speed = 
                        _playerConfig.DodgeStateConfig.DodgeCurve.Evaluate(_playerConfig.DodgeStateConfig.Timer);
                    
                    _playerCompositionRoot.CharacterController.Move(
                        _playerConfig.DodgeStateConfig.LastDodgeDirection * speed * Time.deltaTime);
                }
                else
                {
                    Exit();
                }
            }
                
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction == false)
            {
                //  ON TESTING
                if (_playerCompositionRoot.GroundChecker.isTouches == false)
                {
                    StateSwitcher.SwitchState<FallingState>();
                    return;
                }
                
                if (Data.MovementInput == Vector2.zero)
                {
                    StateSwitcher.SwitchState<IdlingState>();
                    return;
                }

                OnMove();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
            
            _playerConfig.DodgeStateConfig.Timer = 0f;
        }
        #endregion
    }
}