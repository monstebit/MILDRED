using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class RunningStateConfig
    {
        [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; private set; } = 1f;
        
        // [SerializeField, Range(0, 10)] private float _runningSpeed;
        // public float RunningSpeed => _runningSpeed;
    }
}