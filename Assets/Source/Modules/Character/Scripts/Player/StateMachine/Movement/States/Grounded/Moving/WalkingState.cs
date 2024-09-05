using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class WalkingState : MovingState
    {
        private PlayerConfig _playerConfig;

        public WalkingState(
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

            Data.MovementSpeedModifier = _playerConfig.WalkingStateConfig.SpeedModifier; ;
            
            PlayerView.StartWalking();
            
            //  TODO: WeakForce
            // stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopWalking();
        }
        
        public override void Update()
        {
            base.Update();
            
            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
            }

            if (_playerConfig.MovementStateConfig.ShouldWalk)
            {
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
        #endregion
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            //  TODO: LightStoppingState
            // StateSwitcher.SwitchState<LightStoppingState>();

            base.OnMovementCanceled(context);
        }
    }
}