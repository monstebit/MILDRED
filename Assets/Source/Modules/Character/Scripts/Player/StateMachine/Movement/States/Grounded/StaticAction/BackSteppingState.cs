using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class BackSteppingState : StaticActionState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        
        public BackSteppingState(
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
        // public override void Enter()
        // {
        //     _playerConfig.BackSteppingStateConfig.Timer = 0f;
        //     
        //     base.Enter();
        //     
        //     PlayerView.StartBackStepping();
        //     
        //     Keyframe LastFrame = _playerConfig.BackSteppingStateConfig.BackStepCurve[_playerConfig.BackSteppingStateConfig.BackStepCurve.length - 1];
        //     _playerConfig.BackSteppingStateConfig.BackStepTimer = LastFrame.time;
        //     _playerConfig.BackSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
        //     _playerConfig.BackSteppingStateConfig.LastStepDirection.y = 0;
        //     _playerConfig.BackSteppingStateConfig.LastStepDirection.Normalize();
        // }
        //
        // public override void Update()
        // {
        //     base.Update();
        //     
        //     if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
        //     {
        //         _playerConfig.BackSteppingStateConfig.Timer += Time.deltaTime;
        //
        //         if (_playerConfig.BackSteppingStateConfig.Timer < _playerConfig.BackSteppingStateConfig.BackStepTimer)
        //         {
        //             float speed = 
        //                 _playerConfig.BackSteppingStateConfig.BackStepCurve.Evaluate(_playerConfig.BackSteppingStateConfig.Timer);
        //
        //             _playerCompositionRoot.CharacterController.Move(
        //                 -_playerConfig.BackSteppingStateConfig.LastStepDirection * speed * Time.deltaTime);
        //         }
        //         else
        //         {
        //             if (_playerConfig.BackSteppingStateConfig.Timer 
        //                 < _playerConfig.BackSteppingStateConfig.BackStepTimer + _playerConfig.BackSteppingStateConfig.BackStepToStateTimer)
        //             {
        //                 return;
        //             }
        //             
        //             Exit();
        //         }
        //     }
        //     
        //     if (_playerConfig.MovementStateConfig.IsPerformingStaticAction == false) 
        //     {
        //         if (_playerCompositionRoot.GroundChecker.isTouches == false)
        //         {
        //             StateSwitcher.SwitchState<FallingState>();
        //             return;
        //         }
        //         
        //         if (Data.MovementInput == Vector2.zero)
        //         {
        //             StateSwitcher.SwitchState<IdlingState>();
        //             return;
        //         }
        //         
        //         OnMove();
        //     }
        // }
        public override void Enter()
        {
            _playerConfig.BackSteppingStateConfig.Timer = 0f;
            
            base.Enter();
            
            PlayerView.StartBackStepping();
            
            Keyframe lastFrame = _playerConfig.BackSteppingStateConfig.BackStepCurve[_playerConfig.BackSteppingStateConfig.BackStepCurve.length - 1];
            _playerConfig.BackSteppingStateConfig.BackStepTimer = lastFrame.time;
            _playerConfig.BackSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
            _playerConfig.BackSteppingStateConfig.LastStepDirection.y = 0;
            _playerConfig.BackSteppingStateConfig.LastStepDirection.Normalize();
        }

        public override void Update()
        {
            base.Update();
            
            // Логика для статического действия
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
            {
                _playerConfig.BackSteppingStateConfig.Timer += Time.deltaTime;

                if (_playerConfig.BackSteppingStateConfig.Timer < _playerConfig.BackSteppingStateConfig.BackStepTimer)
                {
                    float speed = _playerConfig.BackSteppingStateConfig.BackStepCurve.Evaluate(_playerConfig.BackSteppingStateConfig.Timer);

                    _playerCompositionRoot.CharacterController.Move(
                        -_playerConfig.BackSteppingStateConfig.LastStepDirection * speed * Time.deltaTime);
                }
                else
                {
                    if (_playerConfig.BackSteppingStateConfig.Timer 
                        < _playerConfig.BackSteppingStateConfig.BackStepTimer + _playerConfig.BackSteppingStateConfig.BackStepToStateTimer)
                    {
                        return;  // Завершаем метод, если таймер не завершен
                    }
                    
                    Exit();
                    return;  // Выход из метода после выхода из состояния
                }
            }
            
            // Логика для активного действия (не статического)
            if (!_playerConfig.MovementStateConfig.IsPerformingStaticAction) 
            {
                // Проверка на нахождение на земле
                if (!_playerCompositionRoot.GroundChecker.isTouches)
                {
                    StateSwitcher.SwitchState<FallingState>();
                    return;
                }
                
                // Переход в состояние покоя, если отсутствует ввод
                if (Data.MovementInput == Vector2.zero)
                {
                    StateSwitcher.SwitchState<IdlingState>();
                    return;
                }
                
                // Состояние спринта
                if (_playerConfig.MovementStateConfig.ShouldSprint)
                {
                    StateSwitcher.SwitchState<SprintingState>();
                    return;
                }
            
                // Состояние ходьбы
                if (_playerConfig.MovementStateConfig.ShouldWalk)
                {
                    StateSwitcher.SwitchState<WalkingState>();
                    return;
                }
            
                // Состояние бега
                StateSwitcher.SwitchState<RunningState>();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopBackStepping();
        }
        #endregion
        
        #region OnAmimationEvent Methods
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            Debug.Log("OnAnimationTransitionEvent");
        }
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
        #endregion
        
        //  ON TESTING
        // protected override void OnMovementCanceled(InputAction.CallbackContext context)
        // {
        //     base.OnMovementCanceled(context);
        // }
    }
}