using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing
{
    public class LightLandingState : LandingState
    {
        public LightLandingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = 0f;
        }
    }
}