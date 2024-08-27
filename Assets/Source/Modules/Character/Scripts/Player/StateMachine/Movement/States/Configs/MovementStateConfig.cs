using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class MovementStateConfig
    {
        [Header("INPUTS")]
        [SerializeField] public Vector2 MovementInput;
        [SerializeField] public float MoveAmount;
        [SerializeField] public float VerticalInput;
        [SerializeField] public float HorizontalInput;
        
        [Header("DIRECTIONS")]
        [SerializeField] public Vector3 _targetRotationDirection;
        [SerializeField] public Vector3 _movementDirection;
        [SerializeField] public Vector3 YVelocity;
        
        [Header("MOVEMENT FLAGS")]
        [SerializeField] public bool ShouldWalk;
        [SerializeField] public bool ShouldSprint;
        [SerializeField] public bool IsPerformingAction;
        
        [Header("CAMERA")]
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