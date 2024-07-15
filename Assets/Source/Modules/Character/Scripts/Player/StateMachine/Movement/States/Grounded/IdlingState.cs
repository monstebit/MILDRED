using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class IdlingState : MovingState
    {
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
        }
        
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = 0;
            // Data.MovementSpeedModifier = Data.MovementSpeedModifier;
        }

        public override void Update()
        {
            base.Update();

            if (Data.MovementInput == Vector2.zero)
                return;

            OnMove();
        }
        #endregion
    }
}