using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.PerformingAction
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
            
            _playerConfig.MovementStateConfig.IsPerformiStaticAction = true;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopStaticAction();
            
            _playerConfig.MovementStateConfig.IsPerformiStaticAction = false;
        }
        #endregion
    }
}
