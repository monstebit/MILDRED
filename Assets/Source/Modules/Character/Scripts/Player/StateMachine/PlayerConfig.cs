using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private MovementStateConfig _movementStateConfig;
        [SerializeField] private RunningStateConfig _runningStateConfig;
        [SerializeField] private WalkingStateConfig _walkingStateConfig;
        [SerializeField] private SprintingStateConfig _sprintingStateConfig;
        [SerializeField] private DodgeStateConfig _dodgeStateConfig;
        [SerializeField] private BackSteppingStateConfig _backSteppingStateConfig;
        [SerializeField] private AirborneStateConfig _airborneStateConfig;

        public MovementStateConfig MovementStateConfig => _movementStateConfig;
        public RunningStateConfig RunningStateConfig => _runningStateConfig;
        public WalkingStateConfig WalkingStateConfig => _walkingStateConfig;
        public SprintingStateConfig SprintingStateConfig => _sprintingStateConfig;
        public DodgeStateConfig DodgeStateConfig => _dodgeStateConfig;
        public BackSteppingStateConfig BackSteppingStateConfig => _backSteppingStateConfig;
        public AirborneStateConfig AirborneStateConfig => _airborneStateConfig;
    }
}