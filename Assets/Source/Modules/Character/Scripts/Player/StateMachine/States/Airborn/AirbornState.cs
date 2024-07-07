using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Airborn
{
    public abstract class AirbornState : MovementState
    {
        private readonly AirbornStateConfig _airbornStateConfig;
        
        public AirbornState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement, StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            cameraMovement,
            data)
            => _airbornStateConfig = playerInputHandler.PlayerConfig.AirbornStateConfig;

        public override void Enter()
        {
            base.Enter();

            // Data.Speed = _airbornStateConfig.Speed;
            // Debug.Log($"Скорость в воздухе = {Data.Speed}");
        }

        public override void Update()
        {
            base.Update();
            
            // Data.VerticalInput -= _airbornStateConfig.BaseGravity * Time.deltaTime;
        }
    }
}