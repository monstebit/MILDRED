using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class JumpingStateConfig
    {
        [field: SerializeField] [field: Range(1f, 2f)] public float SpeedModifier { get; set; } = 0.5f;
        
        [SerializeField] public AnimationCurve JumpCurve;
        [SerializeField] public float JumpTimer;
        [SerializeField] public float Timer;
        [SerializeField] public bool IsJumping;
        
        [Header("TEST")]
        [SerializeField] public float JumpForce = 2f;
    }
}