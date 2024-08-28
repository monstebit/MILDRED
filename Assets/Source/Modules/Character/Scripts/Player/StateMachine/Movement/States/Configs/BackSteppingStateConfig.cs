using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class BackSteppingStateConfig
    {
        // [field: SerializeField] [field: Range(0f, 5f)] public float BackStepToAnotherStateTime = 0.5f;
        
        [SerializeField] public AnimationCurve DodgeCurve;
        [SerializeField] public float DodgeTimer;
        [SerializeField] public float timer;
        [SerializeField] public bool isDodging;
        
        [SerializeField] public Vector3 LastStepDirection; // Направление последнего шага назад
        // [SerializeField] public Vector3 StartStepPosition; // Начальная позиция шага назад
        // [SerializeField] public float StartTime; // Время начала шага назад
        // [SerializeField] public float BackStepDuration = 0.5f; // Длительность шага назад
        // [SerializeField] public float BackStepDelay = 0.1f; // ЗАДЕРЖКА ПЕРЕД БЭКСТЕПОМ
        // [SerializeField] public float BackStepSpeed = 1f; // Скорость шага назад
        // [SerializeField] public float BackStepDistance = 2f; // Максимальная дистанция шага назад
    }
}