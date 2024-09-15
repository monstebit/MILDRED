using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing
{
    public class LightLandingState : LandingState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        
        public LightLandingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
        }

        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = 0f;
            
            PlayerView.StartLightLanding();
        }

        public override void Update()
        {
            base.Update();
            
            if (Data.MovementInput != Vector2.zero)
            {
                //  ОСТАНОВКА ИГРОКА ПОСЛЕ ПРЕЗЕМЛЕНИЯ
                // if (InAnimationTransition())
                // {
                //     return;
                // }
                
                OnMove();
                
                return;
            }
            
            StateSwitcher.SwitchState<IdlingState>();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopLightLanding();
        }
        
        //  ON TESTING
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