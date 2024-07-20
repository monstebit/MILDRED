using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class WalkingState : MovingState
    {
        private WalkingStateConfig _walkingStateConfig;
        private SprintingStateConfig _sprintingStateConfig;

        public WalkingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerCameraMovement,
            data)
        {
            _walkingStateConfig = playerInputHandler.PlayerConfig.WalkingStateConfig;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _walkingStateConfig.SpeedModifier; ;
            
            PlayerView.StartWalking();
        }

        public override void Update()
        {
            base.Update();
            
            //  SPRINTING
            if (_sprintingStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                return;
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopWalking();
        }
        #endregion
        
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            
            StateSwitcher.SwitchState<RunningState>();
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
        
        //  SPRINT
        protected override void OnSprintPerformed(InputAction.CallbackContext context)
        {
            base.OnSprintPerformed(context);
        }
        
        protected override void OnSprintCanceled(InputAction.CallbackContext context)
        {
            base.OnSprintCanceled(context);
        }
    }
}