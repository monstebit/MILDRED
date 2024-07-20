using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class MovingState : GroundedState
    {
        private SprintingStateConfig _sprintingStateConfig;
        
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
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
        }

        public override void Update()
        {
            base.Update();

            // if (_sprintingStateConfig.ShouldSprint)
            //     StateSwitcher.SwitchState<SprintingState>();
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartMoving();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopMoving();
        }
        
        // protected override void OnSprintPerformed(InputAction.CallbackContext context)
        // {
        //     StateSwitcher.SwitchState<SprintingState>();
        // }
        //
        // protected override void OnSprintCanceled(InputAction.CallbackContext context)
        // {
        // }
    }
}
