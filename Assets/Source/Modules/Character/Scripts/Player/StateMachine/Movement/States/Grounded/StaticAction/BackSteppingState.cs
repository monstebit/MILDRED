using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public override void Enter()
        {
            base.Enter();

            if (InAnimationTransition())
            {
                return;
            }
            
            _playerConfig.BackSteppingStateConfig.Timer = 0f;
            
            PlayerView.StartBackStepping();
            
            Keyframe lastFrame = _playerConfig.BackSteppingStateConfig.BackStepCurve[_playerConfig.BackSteppingStateConfig.BackStepCurve.length - 1];
            _playerConfig.BackSteppingStateConfig.BackStepTimer = lastFrame.time;
        }

        public override void Exit()
        {
            // Завершаем текущее состояние
            base.Exit();

            // Останавливаем анимацию backstep
            PlayerView.StopBackStepping();
        }

        public override void Update()
        {
            base.Update();

            // Обновляем таймер для backstep
            _playerConfig.BackSteppingStateConfig.Timer += Time.deltaTime;

            if (_playerConfig.BackSteppingStateConfig.Timer < _playerConfig.BackSteppingStateConfig.BackStepTimer)
            {
                // Двигаем персонажа назад с вычисленной скоростью
                float speed = _playerConfig.BackSteppingStateConfig.BackStepCurve.Evaluate(_playerConfig.BackSteppingStateConfig.Timer);
                _playerCompositionRoot.CharacterController.Move(-PlayerView.transform.forward * speed * Time.deltaTime);
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
        #endregion
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
        
        #region OnAmimationEvent Methods
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
        #endregion
    }
}