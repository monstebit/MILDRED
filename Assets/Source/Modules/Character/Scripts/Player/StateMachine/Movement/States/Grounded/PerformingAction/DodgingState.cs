using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.PerformingAction
{
    public class DodgingState : PerformingActionState
    {
        private DodgeStateConfig _dodgeStateConfig;
        private MovementStateConfig _movementStateConfig;
        private PlayerConfig _playerConfig;
        private PlayerCompositionRoot _playerCompositionRoot;
        
        public DodgingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
            {
                _dodgeStateConfig = playerCompositionRoot.PlayerConfig.DodgeStateConfig;
                _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
                _playerCompositionRoot = playerCompositionRoot;
            }
         
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartDodging();
            
            Keyframe LastFrame = _dodgeStateConfig.DodgeCurve[_dodgeStateConfig.DodgeCurve.length - 1];
            _dodgeStateConfig.DodgeTimer = LastFrame.time;
            _dodgeStateConfig.LastDodgeDirection = PlayerView.transform.forward;
            _dodgeStateConfig.LastDodgeDirection.y = 0;
            _dodgeStateConfig.LastDodgeDirection.Normalize();
        }

        public override void Update()
        {
            base.Update();
            
            if (_movementStateConfig.IsPerformingAction)
            {
                _dodgeStateConfig.Timer += Time.deltaTime;

                if (_dodgeStateConfig.Timer < _dodgeStateConfig.DodgeTimer)
                {
                    float speed = 
                        _dodgeStateConfig.DodgeCurve.Evaluate(_dodgeStateConfig.Timer);

                    _playerCompositionRoot.CharacterController.Move(
                        _dodgeStateConfig.LastDodgeDirection * speed * Time.deltaTime);
                }
                else
                {
                    _movementStateConfig.IsPerformingAction = false;
                    _dodgeStateConfig.Timer = 0f;
                }
            }
                
            if (_movementStateConfig.IsPerformingAction == false)
            {
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
            
            _dodgeStateConfig.Timer = 0f;
        }
        #endregion
    }
}