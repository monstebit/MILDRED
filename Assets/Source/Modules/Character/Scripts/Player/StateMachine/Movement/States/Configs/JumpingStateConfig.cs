using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class JumpingStateConfig
    {
        // [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; private set; } = 0.5f;
        [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; set; } = 0.5f;
        // [SerializeField, Range(0, 10)] private float _maxHeight = 5f;
        // [SerializeField, Range(0, 10)] private float _timeToReachMaxHeight = 0.6f;
        
        [Header("STABLE VERSION")]
        [SerializeField] public float JumpForce = 2f;
        
        [Header("TEST")]
        [SerializeField] public AnimationCurve JumpCurve;
        [SerializeField] public float JumpTimer;
        [SerializeField] public float Timer;
        [SerializeField] public bool IsJumping;
    }
}