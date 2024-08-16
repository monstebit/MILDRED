using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class IdlingState : GroundedState
    {
        private PlayerInputHandler _playerInputHandler;
        private MovementStateConfig _movementStateConfig;
        
        public IdlingState(
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
            _playerInputHandler = playerInputHandler;
            _movementStateConfig = _playerInputHandler.PlayerConfig.MovementStateConfig;
        }
        
        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = 0;

            base.Enter();
            
            PlayerView.StartIdling();
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
    }
}