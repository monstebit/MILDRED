using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public abstract class MovementState : IState
    {
        private readonly PlayerInputHandler _playerInputHandler;
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        
        private CharacterNetworkManager _characterNetworkManager;
        private PlayerCameraMovement _playerCameraMovement;
        private MovementStateConfig _movementStateConfig;
        private AirborneStateConfig _airborneStateConfig;
        private DodgeStateConfig _dodgeStateConfig;
        
        protected Vector3 _movementDirection;
        protected Vector3 _targetRotationDirection;
        
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
            _airborneStateConfig = playerInputHandler.PlayerConfig.AirborneStateConfig;
            _dodgeStateConfig = playerInputHandler.PlayerConfig.DodgeStateConfig;
            Data = data;
        }
        
        protected PlayerControls PlayerControls => _playerInputHandler.PlayerControls;
        protected PlayerView PlayerView => _playerInputHandler.PlayerView;

        #region IState METHODS
        public virtual void Enter()
        {
            Debug.Log($"State: {GetType().Name}");
            
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }
        
        public virtual void Update()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            Move();
            Rotate();

            #region JUMP STATE
            Jump();
            #endregion
        }

        public virtual void LateUpdate()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            HandleAllCameraActions();
        }
        #endregion
        
        #region INPUT METHODS
        public virtual void HandleAllInputs()
        {
            HandleMovementInput();
            HandleCameraInput();
        }
        
        private void HandleMovementInput()
        {
            #region DODGE STATE
            if (_movementStateConfig.IsPerformingAction)
                return;
            #endregion
            
            Data.MovementInput = PlayerControls.PlayerMovement.Movement.ReadValue<Vector2>();
            _movementStateConfig.MovementInput = Data.MovementInput;    //  TEST MONITORING
            
            Data.VerticalInput = Data.MovementInput.y;
            _movementStateConfig.VerticalInput = Data.VerticalInput;    //  TEST MONITORING
            Data.HorizontalInput = Data.MovementInput.x;
            _movementStateConfig.HorizontalInput = Data.HorizontalInput;    //  TEST MONITORING
    
            Data.MoveAmount = Mathf.Clamp01(
                Mathf.Abs(Data.VerticalInput) + Mathf.Abs(Data.HorizontalInput));
            _movementStateConfig.MoveAmount = Data.MoveAmount;  //  TEST MONITORING
    
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
        
        protected virtual void AddInputActionsCallbacks()
        {
            PlayerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
            PlayerControls.PlayerMovement.Movement.canceled += OnMovementCanceled;
            PlayerControls.PlayerMovement.WalkToggle.started += OnWalkToggleStarted;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerControls.PlayerMovement.Movement.performed -= OnMovementPerformed;
            PlayerControls.PlayerMovement.Movement.canceled -= OnMovementCanceled;
            PlayerControls.PlayerMovement.WalkToggle.started -= OnWalkToggleStarted;
        }
        
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            _movementStateConfig.ShouldWalk = !_movementStateConfig.ShouldWalk;
        }
        
        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<IdlingState>();
        }
        #endregion

        #region ON ANIMATION EVENT METHODS
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
        private void Move()
        {
            if (_movementStateConfig.IsPerformingAction)
            {
                return;
            }

            #region GROUNDED MOVEMENT
            if (Data.MovementInput == Vector2.zero || Data.MovementSpeedModifier == 0f)
            {
                return;
            }
            
            _movementDirection = GetMovementInputDirection();
            
            float movementSpeed = GetMovementSpeed();
            
            _playerInputHandler.CharacterController.Move(
                _movementDirection * movementSpeed * Time.deltaTime);
            #endregion
        }

        private void Jump()
        {
            // if (!_movementStateConfig.shouldAirborne)
            //     return;
            
            // if (_movementStateConfig.IsPerformingAction)
            //     return;
            
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
            
            Vector3 jumpDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;

            jumpDirection.y = Data.YVelocity;
            
            _playerInputHandler.CharacterController.Move(
                jumpDirection * _airborneStateConfig.JumpingStateConfig.MaxHeight * Time.deltaTime);
        }
        
        public void PerformDodge()
        {
            if (Time.time < _dodgeStateConfig._startTime + _dodgeStateConfig._dodgeDuration &&
                Vector3.Distance(
                    _dodgeStateConfig._startDodgePosition, _playerInputHandler.CharacterController.transform.position) < _dodgeStateConfig._dodgeDistance)
            {
                _playerInputHandler.CharacterController.Move(
                    PlayerView.transform.forward * _dodgeStateConfig._dodgeSpeed * Time.deltaTime);
            }
        }
        
        public void StartDodge()
        {
            _dodgeStateConfig._startDodgePosition = _playerInputHandler.CharacterController.transform.position;
        }
        
        private void Rotate()
        {
            #region DODGE STATE
            if (_movementStateConfig.IsPerformingAction)
                return;
            #endregion
            
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

        private void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleCameraRotation();
        }
        
        private void HandleFollowTarget()
        {
            _playerCameraMovement.transform.position = Vector3.SmoothDamp(
                _playerCameraMovement.transform.position,
                _playerInputHandler.PlayerView.transform.position,
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

        #region REUSABLE METHODS
        private Vector3 GetMovementInputDirection()
        {
            // Получаем правое и переднее направление из положения камеры
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
            // Вычисляем направление движения на основе ввода пользователя
            Vector3 movementDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;
            // Устанавливаем y в 0, чтобы учитывать только горизонтальное движение\
            // movementDirection.y = 0;
            // Устанавливаем y в 0, чтобы учитывать только горизонтальное движение\
            movementDirection.y = Data.YVelocity;
            // Нормализуем направление для получения единичного вектора
            movementDirection.Normalize();
            
            return movementDirection;
        }
        
        private float GetMovementSpeed()
        {
            return Data.BaseSpeed * Data.MovementSpeedModifier;
        }
        #endregion
    }
}