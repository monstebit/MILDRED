using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class StaticActionState : GroundedState
    {
        private PlayerConfig _playerConfig;
        
        public StaticActionState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartStaticAction();
            
            _playerConfig.MovementStateConfig.IsPerformingStaticAction = true;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopStaticAction();
            
            _playerConfig.MovementStateConfig.IsPerformingStaticAction = false;
        }
        #endregion
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            // Не реагируем на отмену движения, пока выполняется статическое действие
        }

        protected override void Move()
        {
            // Не реагируем...
        }
        
        protected override void Rotate()
        {
            // Не реагируем...
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            // Не реагируем...
        }
    }
}
