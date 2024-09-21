using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class IdlingState : GroundedState
    {
        public IdlingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
        }
        
        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = 0;

            base.Enter();
            
            PlayerView.StartIdling();

            ResetShouldWalk();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopIdling();
        }

        public override void Update()
        {
            base.Update();
            
            if (Data.MovementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }
        #endregion
        
        private void ResetShouldWalk()
        {
            PlayerConfig.MovementStateConfig.ShouldWalk = false;
        }
    }
}