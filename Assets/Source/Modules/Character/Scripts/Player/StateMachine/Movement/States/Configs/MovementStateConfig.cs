using System;
using Unity.Collections;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class MovementStateConfig
    {
        [SerializeField] public string ControlScheme;  // Порог времени удержания для спринта
        
        [Header("ЗАЖАТИЕ СПРИНТА И НАЖАТИЕ ДОДЖА")]
        [SerializeField] public float _holdTimeThreshold = 0.2f;  // Порог времени удержания для спринта
        [SerializeField] public float _timeButtonHeld = 0f;       // Время удержания кнопки
        [SerializeField] public bool _isButtonHeld = false;       // Флаг удержания кнопки
        
        [Header("INPUTS")]
        [SerializeField] public Vector2 MovementInput;
        [SerializeField] public float VerticalInput;
        [SerializeField] public float HorizontalInput;
        [SerializeField] public float MoveAmount;
        
        [Header("DIRECTIONS")]
        // [SerializeField] public Vector3 _targetRotationDirection;
        [SerializeField] public Vector3 _movementDirection;
        
        [Header("HANDLE JUMP")]
        [SerializeField] public Vector3 YVelocity;
        [SerializeField] public float GroundedGravityForce = -2f;
        
        [Header("MOVEMENT FLAGS")]
        [SerializeField] public bool ShouldWalk;
        [SerializeField] public bool ShouldSprint;
        // [SerializeField] public bool IsPerformingStaticAction;
        // [SerializeField] public bool IsAirborning ;
        
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