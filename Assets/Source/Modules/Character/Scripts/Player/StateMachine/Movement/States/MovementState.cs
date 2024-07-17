using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public abstract class MovementState : IState
    {
        //
        // protected Vector2 _movementInput;
        protected Vector3 _movementDirection;
        protected Vector3 _targetRotationDirection;

        // protected bool _shouldWalk; //  TODO: NETWORK(?)
        //
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        
        // protected bool IsIdling() => Data.MoveAmount == 0;
        // protected bool IsWalking() => Data.MoveAmount > 0 && Data.MoveAmount <= 0.5f;
        // protected bool IsRunning() => Data.MoveAmount > 0.5f;
        // protected bool IsDodging() => PlayerView.IsPlayerDodging();
        
        private readonly PlayerInputHandler _playerInputHandler;
        
        private CharacterNetworkManager _characterNetworkManager;
        private PlayerCameraMovement _playerCameraMovement;
        private MovementStateConfig _movementStateConfig;
        
        public MovementState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement,
            StateMachineData data)
        {
            StateSwitcher = stateSwitcher;
            _playerInputHandler = playerInputHandler;
            _characterNetworkManager = characterNetworkManager;
            _playerCameraMovement = playerCameraMovement;
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
            Data = data;
        }
        
        protected PlayerControls PlayerControls => _playerInputHandler.PlayerControls;
        protected PlayerView PlayerView => _playerInputHandler.PlayerView;

        #region IState METHODS
        public virtual void Enter()
        {
            Debug.Log($"State: {GetType().Name}");
            
            AddInputActionsCallbacks();
            
            PlayerView.StartMovement();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
            
            PlayerView.StopMovement();
        }
        
        public virtual void Update()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            Move();
            Rotate();
        }

        public virtual void LateUpdate()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            HandleAllCameraActions();
        }
        #endregion

        protected virtual void AddInputActionsCallbacks()
        {
            PlayerControls.PlayerMovement.WalkToggle.started += OnWalkToggleStarted;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerControls.PlayerMovement.WalkToggle.started -= OnWalkToggleStarted;
        }

        #region INPUT METHODS
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            Data.ShouldWalk = !Data.ShouldWalk;
            
            Debug.Log($" НАЖАЛ {Data.ShouldWalk}");
        }
        
        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }
        #endregion
        
        #region MAIN METHODS
        public virtual void HandleAllInputs()
        {
            HandleMovementInput();
            HandleCameraInput();
        }
        
        private void HandleMovementInput()
        {
            Data.MovementInput = PlayerControls.PlayerMovement.Movement.ReadValue<Vector2>();
            
            Data.VerticalInput = Data.MovementInput.y;
            Data.HorizontalInput = Data.MovementInput.x;
    
            Data.MoveAmount = Mathf.Clamp01(Mathf.Abs(Data.VerticalInput) + Mathf.Abs(Data.HorizontalInput));
    
            if (Data.MoveAmount <= 0.5 && Data.MoveAmount > 0)
            {
                Data.MoveAmount = 0.5f;
            }
            else if (Data.MoveAmount > 0.5 && Data.MoveAmount <= 1)
            {
                Data.MoveAmount = 1;
            }
        }

        private void HandleCameraInput()
        {
            Data.CameraInput = PlayerControls.PlayerCamera.Movement.ReadValue<Vector2>();
            Data.CameraVerticalInput = Data.CameraInput.y;
            Data.CameraHorizontalInput = Data.CameraInput.x;
            
            if (_playerInputHandler.IsOwner)
            {
                _characterNetworkManager.NetworkPosition.Value = _playerInputHandler.transform.position;
                _characterNetworkManager.NetworkRotation.Value = _playerInputHandler.transform.rotation;
            }
            else
            {
                _playerInputHandler.transform.position = Vector3.SmoothDamp(
                    _playerInputHandler.transform.position,
                    _characterNetworkManager.NetworkPosition.Value,
                    ref _characterNetworkManager.NetworkPositionVelocity,
                    _characterNetworkManager.NetworkPositionSmoothTime);

                _playerInputHandler.transform.rotation = Quaternion.Slerp(_playerInputHandler.transform.rotation,
                    _characterNetworkManager.NetworkRotation.Value, _characterNetworkManager.NetworkRotationSmoothTime);
            }
        }
        
        #region CAMERA METHODS
        private void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleCameraRotation();
        }
        
        private void HandleFollowTarget()
        {
            _playerCameraMovement.transform.position = Vector3.SmoothDamp(
                _playerCameraMovement.transform.position,
                _playerInputHandler.PlayerView.transform.position,   //  КОРРЕКТНАЯ РАБОТА С ApplyRootMotion
                ref _playerCameraMovement.CameraVelocity,
                _playerCameraMovement.CameraSmoothSpeed * Time.deltaTime);
        }
        
        private void HandleCameraRotation()
        {
            Quaternion playerCameraPivotRotation = _playerCameraMovement.CameraPivotTransform.rotation;
            _playerCameraMovement.PlayerCameraYRotation += Data.CameraHorizontalInput * _movementStateConfig.Sensitivity;
            _playerCameraMovement.PlayerCameraXRotation -= Data.CameraVerticalInput * _movementStateConfig.Sensitivity;;
            _playerCameraMovement.PlayerCameraXRotation = Mathf.Clamp(
                _playerCameraMovement.PlayerCameraXRotation, 
                _movementStateConfig.MinimumPivot, 
                _movementStateConfig.MaximumPivot);
            
            playerCameraPivotRotation = Quaternion.Euler(
                _playerCameraMovement.PlayerCameraXRotation,
                _playerCameraMovement.PlayerCameraYRotation,
                playerCameraPivotRotation.eulerAngles.z);
            
            _playerCameraMovement.CameraPivotTransform.localRotation = playerCameraPivotRotation;
        }
        #endregion
        
        private void Move()
        {
            if (Data.MovementInput == Vector2.zero)
                return;

            _movementDirection = GetMovementInputDirection();
            
            float movementSpeed = GetMovementSpeed();
            
            _playerInputHandler.CharacterController.Move(_movementDirection * movementSpeed * Time.deltaTime);
        }
        // private void Move()
        // {
        //     Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
        //     Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
        //     
        //     //   МЫ НЕ МОЖЕМ ПЕРЕДВИГАТЬСЯ ПРИ ПЕРЕКАТЕ
        //     // if (IsDodging())
        //     // {
        //     //     Vector3 dodgeDirection = _movementStateConfig.MoveDirection * _movementStateConfig.DodgeDistance; // толкаем на 2 метра в направлении движения
        //     //     _playerInputHandler.CharacterController.Move(dodgeDirection * Time.deltaTime);
        //     //     
        //     //     return;
        //     // }
        //     
        //     //  ПЕРЕДВИЖЕНИЕ ПО ЗЕМЛЕ
        //     _movementStateConfig.MoveDirection = forward * Data.VerticalInput + right * Data.HorizontalInput;
        //     // _moveDirection.y = 0;
        //     _movementStateConfig.MoveDirection.y = Data.YVelocity;  //  ПРЫЖОК (ВЕРТИКАЛЬ)
        //     _movementStateConfig.MoveDirection.Normalize();
        //
        //     _playerInputHandler.CharacterController.Move(_movementStateConfig.MoveDirection * Data.Speed * Time.deltaTime);
        // }
        
        #region REUSABLE METHODS
        protected Vector3 GetMovementInputDirection()
        {
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;

            Vector3 movementDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;

            movementDirection.y = 0;
            movementDirection.Normalize();

            return movementDirection;
        }
        
        protected float GetMovementSpeed()
        {
            return Data.BaseSpeed * Data.MovementSpeedModifier;
        }
        #endregion
        
        private void Rotate()
        {
            // if (IsDodging())    //  !!!TEST
            //     return;
            
            Transform cameraObjectTransform = _playerCameraMovement.CameraObject.transform;

            Vector3 cameraObjectForward = cameraObjectTransform.forward;
            Vector3 cameraObjectRight = cameraObjectTransform.right;

            _targetRotationDirection = cameraObjectForward * Data.VerticalInput + cameraObjectRight * Data.HorizontalInput;
            _targetRotationDirection.y = 0;
            _targetRotationDirection.Normalize();

            if (_targetRotationDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
                
                Quaternion targetRotation = Quaternion.Slerp(
                    PlayerView.transform.rotation,
                    newRotation,
                    _movementStateConfig.RotationSpeed * Time.deltaTime);
                
                PlayerView.transform.rotation = targetRotation;
            }
        }
        #endregion
    }
}