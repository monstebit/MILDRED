using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class WalkingStateConfig
    {
        [field: SerializeField] [field: Range(0f, 1f)] public float SpeedModifier { get; private set; } = 0.225f;
        
        // [SerializeField, Range(0, 10)] private float _walkingSpeed;
        // public float WalkingSpeed => _walkingSpeed;
    }
}