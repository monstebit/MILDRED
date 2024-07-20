using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class SprintingStateConfig
    {
        // [SerializeField] public float _startTime;
        // [SerializeField] public bool _keepSprinting; //  В ГЕНЩИНЕ СПРИНТ ПЛАВНО ПЕРЕХОДИТ В БЕГ ЧЕРЕЗ ПАРУ СЕКУНД
        // [SerializeField] public bool _shouldResetSprintState;
        [SerializeField] public bool ShouldSprint;
        
        [field: SerializeField] [field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 1.7f;
        // [field: SerializeField] [field: Range(0f, 5f)] public float SprintToRunTime { get; private set; } = 1f;
        [field: SerializeField] [field: Range(0f, 5f)] public float RunToWalkTime { get; private set; } = 0.5f;
    }
}