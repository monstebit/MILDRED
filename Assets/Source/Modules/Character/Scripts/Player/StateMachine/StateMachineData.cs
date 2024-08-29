using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    public class StateMachineData
    {
        public Vector2 CameraInput { get; set; }
        public Vector2 MovementInput { get; set; }
        public float BaseSpeed { get; set; } = 5f;
        public float MovementSpeedModifier { get; set; } = 1f;

        private Vector3 _currentTargetRotation;
        public ref Vector3 CurrentTargetRotation
        {
            get
            {
                return ref _currentTargetRotation;
            }
        }

        public float XVelocity;
        private float _verticalInput;
        private float _horizontalInput;
        private float _cameraVerticalInput;
        private float _cameraHorizontalInput;
        private float _moveAmount;
        private float _speed;
        private Vector3 dampedTargetRotationCurrentVelocity;

        
        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }
        
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
        // public float Speed
        // {
        //     get => _speed;
        //     set
        //     {
        //         if (value < 0)
        //             throw new ArgumentOutOfRangeException(nameof(value), "Speed cannot be negative");
        //
        //         _speed = value;
        //     }
        // }
    }
}