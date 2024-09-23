using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class BackSteppingStateConfig
    {
        [field: SerializeField] [field: Range(0f, 5f)] public float BackStepToMoveTime { get; private set; } = 0.5f;
        [SerializeField] public AnimationCurve BackStepCurve;
        [SerializeField] public float BackStepTimer;
        [SerializeField] public float Timer;
    }
}