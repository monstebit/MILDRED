using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private GroundedStateConfig _groundedStateConfig;
        [SerializeField] private MovementStateConfig _movementStateConfig;
        [SerializeField] private WalkingStateConfig _walkingStateConfig;
        [SerializeField] private RunningStateConfig _runningStateConfig;
        [SerializeField] private AirborneStateConfig _airborneStateConfig;

        public GroundedStateConfig GroundedStateConfig => _groundedStateConfig;
        public MovementStateConfig MovementStateConfig => _movementStateConfig;
        public WalkingStateConfig WalkingStateConfig => _walkingStateConfig;
        public RunningStateConfig RunningStateConfig => _runningStateConfig;
        public AirborneStateConfig AirborneStateConfig => _airborneStateConfig;
    }
}