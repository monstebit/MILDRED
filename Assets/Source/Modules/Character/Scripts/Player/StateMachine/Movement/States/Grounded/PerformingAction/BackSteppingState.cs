using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.PerformingAction
{
    public class BackSteppingState : PerformingActionState
    {
        private MovementStateConfig _movementStateConfig;
        private BackSteppingStateConfig _backSteppingStateConfig;
        private PlayerCompositionRoot _playerCompositionRoot;
        
        public BackSteppingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _movementStateConfig = playerCompositionRoot.PlayerConfig.MovementStateConfig;
            _backSteppingStateConfig = playerCompositionRoot.PlayerConfig.BackSteppingStateConfig;
            _playerCompositionRoot = playerCompositionRoot;
        }
    
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartBackStepping();
            
            Keyframe LastFrame = _backSteppingStateConfig.BackStepCurve[_backSteppingStateConfig.BackStepCurve.length - 1];
            _backSteppingStateConfig.BackStepTimer = LastFrame.time;
            _backSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
            _backSteppingStateConfig.LastStepDirection.y = 0;
            _backSteppingStateConfig.LastStepDirection.Normalize();
        }

        public override void Update()
        {
            base.Update();
            
            if (_movementStateConfig.IsPerformingAction)
            {
                _backSteppingStateConfig.Timer += Time.deltaTime;

                if (_backSteppingStateConfig.Timer < _backSteppingStateConfig.BackStepTimer)
                {
                    float speed = 
                        _backSteppingStateConfig.BackStepCurve.Evaluate(_backSteppingStateConfig.Timer);

                    _playerCompositionRoot.CharacterController.Move(
                        -_backSteppingStateConfig.LastStepDirection * speed * Time.deltaTime);
                }
                else
                {
                    _movementStateConfig.IsPerformingAction = false;
                    _backSteppingStateConfig.Timer = 0;
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
            
            PlayerView.StopBackStepping();

            _backSteppingStateConfig.Timer = 0f;
        }
        #endregion
    }
}