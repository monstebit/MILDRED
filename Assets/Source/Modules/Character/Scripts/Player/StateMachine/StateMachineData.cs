using System;
using Unity.Collections;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    public class StateMachineData
    {
        // public FixedString128Bytes ControlScheme { get; set; }
        // public string ControlScheme;
        public string ControlScheme;
        public Vector2 CameraInput { get; set; }
        public Vector2 MovementInput { get; set; }
        public float BaseSpeed { get; set; } = 5f;
        public float MovementSpeedModifier { get; set; } = 1f;
        public float JumpModifier { get; set; } = 1f;
        
        private Vector3 _currentTargetRotation;
        private float _verticalInput;
        private float _horizontalInput;
        private float _cameraVerticalInput;
        private float _cameraHorizontalInput;
        private float _moveAmount;
        private float _speed;
        private Vector3 dampedTargetRotationCurrentVelocity;
        
        public float VerticalInput
        {
            get => _verticalInput;
            set
            {
                // if (value < -1 || value > 1)
                //     throw new ArgumentOutOfRangeException(nameof(value), "Vertical input must be between -1 and 1");
        
                _verticalInput = value;
            }
        }
        public float HorizontalInput
        {
            get => _horizontalInput;
            set
            {
                // if (value < -1 || value > 1)
                //     throw new ArgumentOutOfRangeException(nameof(value), "Horizontal input must be between -1 and 1");
        
                _horizontalInput = value;
            }
        }
        public float CameraVerticalInput
        {
            get => _cameraVerticalInput;
            set
            {
                // if (value < -1 || value > 1)
                //     throw new ArgumentOutOfRangeException(nameof(value), "Camera vertical input must be between -1 and 1");
        
                _cameraVerticalInput = value;
            }
        }
        public float CameraHorizontalInput
        {
            get => _cameraHorizontalInput;
            set
            {
                // if (value < -1 || value > 1)
                //     throw new ArgumentOutOfRangeException(nameof(value), "Camera horizontal input must be between -1 and 1");
        
                _cameraHorizontalInput = value;
            }
        }
        public float MoveAmount
        {
            get => _moveAmount;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Move amount cannot be negative");
        
                _moveAmount = value;
            }
        }
    }
}