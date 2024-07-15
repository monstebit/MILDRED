using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class SprintingState : MovingState
    {
        public SprintingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager, 
            PlayerCameraMovement playerPlayerCameraMovement, 
            StateMachineData data) : base(stateSwitcher, 
            playerInputHandler, characterNetworkManager, 
            playerPlayerCameraMovement, 
            data)
        {
        }
    }
}