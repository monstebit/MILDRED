using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private WalkingStateConfig _walkingStateConfig;
        [SerializeField] private SprintingStateConfig _sprintingStateConfig;

        public WalkingStateConfig WalkingStateConfig => _walkingStateConfig;
        public SprintingStateConfig SprintingStateConfig => _sprintingStateConfig;
    }
}