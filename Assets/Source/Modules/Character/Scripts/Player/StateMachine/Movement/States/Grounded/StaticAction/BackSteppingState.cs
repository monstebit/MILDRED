using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class BackSteppingState : StaticActionState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        private float _startTime;
        
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
    
        public override void Enter()
        {
            base.Enter();
            
            if (InAnimationTransition())
            {
                return;
            }
            
            PlayerView.UpdateState("IsBackStepping", true);
            
            _startTime = Time.time;
            _playerConfig.BackSteppingStateConfig.Timer = 0f;
            Keyframe lastFrame = _playerConfig.BackSteppingStateConfig.BackStepCurve[_playerConfig.BackSteppingStateConfig.BackStepCurve.length - 1];
            _playerConfig.BackSteppingStateConfig.BackStepTimer = lastFrame.time;
        }

        public override void Exit()
        {
            base.Exit();

            PlayerView.UpdateState("IsBackStepping", false);
        }

        public override void Update()
        {
            base.Update();

            HandleBackStep();
        }
        
        private void HandleBackStep()
        {
            // Обновляем таймер для backstep
            _playerConfig.BackSteppingStateConfig.Timer += Time.deltaTime;

            if (_playerConfig.BackSteppingStateConfig.Timer < _playerConfig.BackSteppingStateConfig.BackStepTimer)
            {
                // Двигаем персонажа назад с вычисленной скоростью
                float speed = _playerConfig.BackSteppingStateConfig.BackStepCurve.Evaluate(_playerConfig.BackSteppingStateConfig.Timer);
                _playerCompositionRoot.CharacterController.Move(-PlayerView.transform.forward * speed * Time.deltaTime);
                return;
            }

            if (Time.time < _startTime + _playerConfig.BackSteppingStateConfig.BackStepToMoveTime)
            {
                return;
            }
            
            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                StateSwitcher.SwitchState<FallingState>();
                return;
            }

            // Проверка на ввод движения перед выходом из состояния
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
                return;
            }
            
            OnMove();
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}