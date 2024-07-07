using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Airborn
{
    public class FallingState : AirbornState
    {
        private readonly GroundChecker _groundChecker;

        public FallingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement,
            StateMachineData data) : base(
            stateSwitcher, playerInputHandler,
            characterNetworkManager,
            cameraMovement,
            data)
            => _groundChecker = playerInputHandler.GroundChecker;

        public override void Update()
        {
            base.Update();

            // if (_groundChecker.isTouches)
            // {
            //     Debug.Log("isTouches");
            //     if (IsPlayerIdling())
            //     {
            //         StateSwitcher.SwitchState<IdlingState>();
            //     }
            //     else if (IsPlayerWalking())
            //     {
            //         StateSwitcher.SwitchState<WalkingState>();
            //     }
            //     else if (IsPlayerSprinting())
            //     {
            //         StateSwitcher.SwitchState<SprintingState>();
            //     }
            // }
        }
    }
}