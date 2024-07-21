using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private MovementStateConfig _movementStateConfig;
        [SerializeField] private WalkingStateConfig _walkingStateConfig;
        [SerializeField] private RunningStateConfig _runningStateConfig;
        [SerializeField] private SprintingStateConfig _sprintingStateConfig;
        [SerializeField] private DodgeStateConfig _dodgeStateConfig;
        [SerializeField] private AirborneStateConfig _airborneStateConfig;

        public MovementStateConfig MovementStateConfig => _movementStateConfig;
        public WalkingStateConfig WalkingStateConfig => _walkingStateConfig;
        public RunningStateConfig RunningStateConfig => _runningStateConfig;
        public SprintingStateConfig SprintingStateConfig => _sprintingStateConfig;
        public DodgeStateConfig DodgeStateConfig => _dodgeStateConfig;
        public AirborneStateConfig AirborneStateConfig => _airborneStateConfig;
    }
}