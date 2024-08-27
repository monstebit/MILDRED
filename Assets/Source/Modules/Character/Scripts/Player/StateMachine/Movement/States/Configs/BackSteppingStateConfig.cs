using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class BackSteppingStateConfig
    {
        [SerializeField] public float StepBackDelay = 0.1f; // ЗАДЕРЖКА ПЕРЕД БЭКСТЕПОМ
        [SerializeField] public float StepBackSpeed = 1f; // Скорость шага назад
        [SerializeField] public float StepBackDuration = 0.5f; // Длительность шага назад
        [SerializeField] public float StepBackDistance = 2f; // Максимальная дистанция шага назад
        [SerializeField] public Vector3 _lastStepBackDirection; // Направление последнего шага назад
        [SerializeField] public Vector3 _startStepBackPosition; // Начальная позиция шага назад
        [SerializeField] public float _startTime; // Время начала шага назад
    }
}