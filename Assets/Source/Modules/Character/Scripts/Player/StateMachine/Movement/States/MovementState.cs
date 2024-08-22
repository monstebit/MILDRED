using System.Collections;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public abstract class MovementState : IState
    {
        private float stepProgress;
        
        private readonly PlayerInputHandler _playerInputHandler;
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        
        private CharacterNetworkManager _characterNetworkManager;
        private PlayerCameraMovement _playerCameraMovement;
        private MovementStateConfig _movementStateConfig;
        private AirborneStateConfig _airborneStateConfig;
        private DodgeStateConfig _dodgeStateConfig;
        private BackSteppingStateConfig _backSteppingStateConfig;
        
        public Vector3 _movementDirection;
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
            _backSteppingStateConfig = playerInputHandler.PlayerConfig.BackSteppingStateConfig;
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
            HandleVerticalMovement();
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
            // if (_movementStateConfig.IsPerformingAction)
            //     return;
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
            // PlayerControls.PlayerMovement.WalkToggle.started += OnWalkToggleStarted;
            PlayerControls.PlayerMovement.WalkToggle.performed += OnWalkToggleStarted;
            PlayerControls.PlayerMovement.WalkToggle.canceled += OnWalkToggleCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerControls.PlayerMovement.Movement.performed -= OnMovementPerformed;
            PlayerControls.PlayerMovement.Movement.canceled -= OnMovementCanceled;
            // PlayerControls.PlayerMovement.WalkToggle.started -= OnWalkToggleStarted;
            PlayerControls.PlayerMovement.WalkToggle.performed -= OnWalkToggleStarted;
            PlayerControls.PlayerMovement.WalkToggle.canceled -= OnWalkToggleCanceled;
        }
        
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            _movementStateConfig.ShouldWalk = true;
        }
        
        protected virtual void OnWalkToggleCanceled(InputAction.CallbackContext context)
        {
            _movementStateConfig.ShouldWalk = false;
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
        
        private void Move()
        {
            if (_movementStateConfig.IsPerformingAction)
            {
                return;
            }

            if (Data.MovementInput == Vector2.zero || Data.MovementSpeedModifier == 0f)
            {
                return;
            }

            if (_playerInputHandler.GroundChecker.isTouches == false)
            {
                return;
            }
            
            _movementDirection = GetMovementInputDirection();
            
            float movementSpeed = GetMovementSpeed();
            
            _playerInputHandler.CharacterController.Move(
                _movementDirection * movementSpeed * Time.deltaTime);
        }
        
        public void HandleVerticalMovement()
        {
            _playerInputHandler.CharacterController.Move(_movementStateConfig.YVelocity * Time.deltaTime);
        }


        #region BackStep

        public void PerformBackStep()
        {
            if (IsBackStepInProgress() && HasNotExceededBackStepDistance())
            {
                MoveCharacterBackward();
            }
            else
            {
                EndBackStep();
            }
        }

        public void MoveCharacterBackward()
        {
            if (_backSteppingStateConfig._lastDodgeDirection != Vector3.zero)
            {
                _playerInputHandler.CharacterController.Move(
                    -_backSteppingStateConfig._lastDodgeDirection * _backSteppingStateConfig.StepBackSpeed * Time.deltaTime);
            }
            else
            {
                EndBackStep();
            }
        }
        
        private bool HasNotExceededBackStepDistance()
        {
            return Vector3.Distance(
                _backSteppingStateConfig._startStepBackPosition, 
                _playerInputHandler.CharacterController.transform.position) < _backSteppingStateConfig.StepBackDistance;
        }
        
        public void StartBackStep()
        {
            _backSteppingStateConfig._lastDodgeDirection = PlayerView.transform.forward;
            _backSteppingStateConfig._lastDodgeDirection.y = 0;
            _backSteppingStateConfig._lastDodgeDirection.Normalize();

            if (_backSteppingStateConfig._lastDodgeDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_backSteppingStateConfig._lastDodgeDirection);
                PlayerView.transform.rotation = newRotation;
            }
            
            _backSteppingStateConfig._startTime = Time.time;
            _backSteppingStateConfig._startStepBackPosition = _playerInputHandler.CharacterController.transform.position;
            
            _movementStateConfig.IsPerformingAction = true;
        }
        
        private bool IsBackStepInProgress()
        {
            return Time.time < _backSteppingStateConfig._startTime + _backSteppingStateConfig.StepBackDuration;
        }
        
        private void EndBackStep()
        {
            _movementStateConfig.IsPerformingAction = false;
        }
        
        #endregion


        
        

        #region DODGE
        public void StartDodge()
        {
            // Сохраняем текущее направление кувырка
            Transform cameraObjectTransform = _playerCameraMovement.CameraObject.transform;

            Vector3 cameraObjectForward = cameraObjectTransform.forward;
            Vector3 cameraObjectRight = cameraObjectTransform.right;

            _movementStateConfig._lastDodgeDirection = cameraObjectForward * Data.VerticalInput + cameraObjectRight * Data.HorizontalInput;
            _movementStateConfig._lastDodgeDirection.y = 0; // Убираем вертикальный компонент
            _movementStateConfig._lastDodgeDirection.Normalize();

            if (_movementStateConfig._lastDodgeDirection != Vector3.zero)
            {
                // Устанавливаем начальное вращение персонажа в направлении кувырка
                Quaternion newRotation = Quaternion.LookRotation(_movementStateConfig._lastDodgeDirection);
                PlayerView.transform.rotation = newRotation;
            }

            _dodgeStateConfig._startTime = Time.time;
            _dodgeStateConfig._startDodgePosition = _playerInputHandler.CharacterController.transform.position;
            _movementStateConfig.IsPerformingAction = true;
        }

        public void PerformDodge()
        {
            if (IsDodgeInProgress() && HasNotExceededDodgeDistance())
            {
                MoveCharacterForward();
            }
            else
            {
                EndDodge();
            }
        }

        private void EndDodge()
        {
            _movementStateConfig.IsPerformingAction = false;
        }

        private bool IsDodgeInProgress()
        {
            return Time.time < _dodgeStateConfig._startTime + _dodgeStateConfig._dodgeDuration;
        }

        private bool HasNotExceededDodgeDistance()
        {
            return Vector3.Distance(
                _dodgeStateConfig._startDodgePosition, 
                _playerInputHandler.CharacterController.transform.position) < _dodgeStateConfig._dodgeDistance;
        }
        
        private void MoveCharacterForward()
        {
            // Используем сохраненное направление для движения персонажа
            if (_movementStateConfig._lastDodgeDirection != Vector3.zero)
            {
                _playerInputHandler.CharacterController.Move(
                    _movementStateConfig._lastDodgeDirection * _dodgeStateConfig._dodgeSpeed * Time.deltaTime);
            }
            else
            {
                // Если нет сохраненного направления, завершаем кувырок
                EndDodge();
            }
        }
        #endregion
        
        
        
        
        
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

        #region REUSABLE METHODS
        public Vector3 GetMovementInputDirection()
        {
            // Получаем правое и переднее направление из положения камеры
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
            // Вычисляем направление движения на основе ввода пользователя
            Vector3 movementDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;
            // Устанавливаем y в 0, чтобы учитывать только горизонтальное движение\
            // movementDirection.y = 0;
            
            //  TODO
            // movementDirection.y = Data.YVelocity;
            // movementDirection.y = _movementStateConfig.YVelocity.y;
            movementDirection.y = 0;
            
            
            // Нормализуем направление для получения единичного вектора
            movementDirection.Normalize();
            
            return movementDirection;
        }
        
        public float GetMovementSpeed()
        {
            return Data.BaseSpeed * Data.MovementSpeedModifier;
        }
        #endregion
    }
}