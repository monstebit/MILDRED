using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.PerformingAction
{
    public class PerformingActionState : GroundedState
    {
        private MovementStateConfig _movementStateConfig;
        
        public PerformingActionState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerPlayerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerPlayerCameraMovement,
            data)
        {
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            PlayerView.StartPerformingAction();
            
            _movementStateConfig.IsPerformingAction = true;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopPerformingAction();
            
            _movementStateConfig.IsPerformingAction = false;
        }
        #endregion
    }
}
