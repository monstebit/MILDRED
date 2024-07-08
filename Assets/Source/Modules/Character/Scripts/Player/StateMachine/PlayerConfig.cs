using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private WalkingStateConfig _walkingStateConfig;
        [SerializeField] private RunningStateConfig runningStateConfig;
        [SerializeField] private AirborneStateConfig airborneStateConfig;

        public WalkingStateConfig WalkingStateConfig => _walkingStateConfig;
        public RunningStateConfig RunningStateConfig => runningStateConfig;
        public AirborneStateConfig AirborneStateConfig => airborneStateConfig;
    }
}