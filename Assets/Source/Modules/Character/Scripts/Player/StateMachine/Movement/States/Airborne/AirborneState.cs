using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne
{
    public abstract class AirborneState : MovementState
    {
        private readonly AirborneStateConfig _airborneStateConfig;
        
        public AirborneState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement, StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerCameraMovement,
            data)
            => _airborneStateConfig = playerInputHandler.PlayerConfig.AirborneStateConfig;

        public override void Enter()
        {
            base.Enter();

            // Data.Speed = _airborneStateConfig.Speed;
            
            PlayerView.StartAirborne();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopAirborne();
        }

        public override void Update()
        {
            base.Update();

            // Data.YVelocity -= _airborneStateConfig.BaseGravity * Time.deltaTime;
        }
    }
}