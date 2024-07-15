using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class MovingState : GroundedState
    {
        // Start is called before the first frame update
        public MovingState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager, 
            PlayerCameraMovement playerPlayerCameraMovement, 
            StateMachineData data) : base(
            stateSwitcher, playerInputHandler, 
            characterNetworkManager, 
            playerPlayerCameraMovement, 
            data)
        {
        }
    }
}
