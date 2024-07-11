using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Configs
{
    [Serializable]
    public class MovementStateConfig
    {
        //
        public bool isDodging = false;
        public bool IsPerformingAction = false;
        public float RollDistance = 5f;
        //
        public Vector3 MoveDirection;
        public Vector3 TargetRotationDirection;
        
        [SerializeField, Range(0.1f, 10f)] private float _sensitivity = 1.5f;
        [SerializeField, Range(-90, 0)] private float _minimumPivot = -30;
        [SerializeField, Range(0, 90)] private float _maximumPivot = 80;
        [SerializeField, Range(0, 50)] private float _rotationSpeed = 15;
        
        public float Sensitivity => _sensitivity;
        public float MinimumPivot => _minimumPivot;
        public float MaximumPivot => _maximumPivot;
        public float RotationSpeed => _rotationSpeed;
    }
}