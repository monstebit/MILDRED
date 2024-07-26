using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class MovementStateConfig
    {
        #region TEST FLAGS
        [Header("TEST FLAGS")]
        [SerializeField] public bool ShouldWalk;
        [SerializeField] public bool ShouldSprint;
        
        [SerializeField] public bool IsPerformingAction;
        // [SerializeField] public bool shouldDodge;
        // [SerializeField] public bool shouldAirborne;
        [SerializeField] public bool endAnimationDodge;
        
        [Header("INPUTS")]
        [SerializeField] public float MoveAmount;
        [SerializeField] public Vector2 MovementInput;
        [SerializeField] public float VerticalInput;
        [SerializeField] public float HorizontalInput;
        #endregion
        
        [Header("ROTATION")]
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