using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.PerformingAction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public abstract class MovementState : IState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly PlayerCameraMovement _playerCameraMovement;
        private readonly CharacterNetworkManager _characterNetworkManager;
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        protected readonly PlayerConfig _playerConfig;
        
        public MovementState(
            IStateSwitcher stateSwitcher, 
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data)
        {
            StateSwitcher = stateSwitcher;
            _playerCompositionRoot = playerCompositionRoot;
            _playerConfig = playerCompositionRoot.PlayerConfig;
            _playerCameraMovement = playerCompositionRoot.PlayerCameraMovement;
            _characterNetworkManager = playerCompositionRoot.CharacterNetworkManager;
            Data = data;
        }
        
        protected PlayerControls PlayerControls => _playerCompositionRoot.PlayerControls;
        protected PlayerView PlayerView => _playerCompositionRoot.PlayerView;

        #region IState METHODS
        public virtual void Enter()
        {
            Debug.Log($"State: {GetType().Name}");
            // Debug.Log($"Speed Modifier: {Data.MovementSpeedModifier}");
            
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }
        
        public virtual void Update()
        {
            if (!_playerCompositionRoot.CharacterNetworkManager.IsOwner)
            {
                return;
            }
            
            Move();
            Rotate();
            HandleVerticalMovement();
            
            
            
            //  ОТСЛЕЖОВАНИЕ УДЕРЖАНИЯ СПРИНТА
            if (_playerConfig.MovementStateConfig._isButtonHeld)
            {
                _playerConfig.MovementStateConfig._timeButtonHeld += Time.deltaTime;

                if (_playerConfig.MovementStateConfig._timeButtonHeld >= _playerConfig.MovementStateConfig._holdTimeThreshold)
                {
                    if (!_playerConfig.MovementStateConfig.ShouldSprint)
                    {
                        // Запуск спринта после достижения порога времени удержания
                        _playerConfig.MovementStateConfig.ShouldSprint = true;
                        // Debug.Log("Начат спринт");
                    }
                }
            }
        }

        public virtual void LateUpdate()
        {
            if (!_playerCompositionRoot.CharacterNetworkManager.IsOwner)
                return;
            
            HandleAllCameraActions();
        }
        #endregion
        
        #region INPUT METHODS
        //  TODO: ПЕРЕНЕСТИ В ОТДЕЛЬНЫЙ КЛАСС PlayerInputHandler
        public virtual void HandleAllInputs()
        {
            HandleMovementInput();
            HandleCameraInput();
        }
        
        protected virtual void HandleMovementInput()
        {
            Data.MovementInput = PlayerControls.Player.Move.ReadValue<Vector2>();
            _playerConfig.MovementStateConfig.MovementInput = Data.MovementInput;    //  TEST MONITORING
            
            Data.VerticalInput = Data.MovementInput.y;
            _playerConfig.MovementStateConfig.VerticalInput = Data.VerticalInput;    //  TEST MONITORING
            
            Data.HorizontalInput = Data.MovementInput.x;
            _playerConfig.MovementStateConfig.HorizontalInput = Data.HorizontalInput;    //  TEST MONITORING
    
            Data.MoveAmount = Mathf.Clamp01(
                Mathf.Abs(Data.VerticalInput) + Mathf.Abs(Data.HorizontalInput));
            _playerConfig.MovementStateConfig.MoveAmount = Data.MoveAmount;  //  TEST MONITORING
    
            if (Data.MoveAmount <= 0.5 && Data.MoveAmount > 0)
            {
                Data.MoveAmount = 0.5f;
            }
            else if (Data.MoveAmount > 0.5 && Data.MoveAmount <= 1)
            {
                Data.MoveAmount = 1;
            }
        }

        protected virtual void HandleCameraInput()
        {
            Data.CameraInput = PlayerControls.Player.Look.ReadValue<Vector2>();
            Data.CameraVerticalInput = Data.CameraInput.y;
            Data.CameraHorizontalInput = Data.CameraInput.x;
            
            // if (_playerInputHandler.IsOwner)
            if (!_playerCompositionRoot.CharacterNetworkManager.IsOwner)
            {
                _characterNetworkManager.NetworkPosition.Value = _playerCompositionRoot.transform.position;
                _characterNetworkManager.NetworkRotation.Value = _playerCompositionRoot.transform.rotation;
            }
            else
            {
                _playerCompositionRoot.transform.position = Vector3.SmoothDamp(
                    _playerCompositionRoot.transform.position,
                    _characterNetworkManager.NetworkPosition.Value,
                    ref _characterNetworkManager.NetworkPositionVelocity,
                    _characterNetworkManager.NetworkPositionSmoothTime);

                _playerCompositionRoot.transform.rotation = Quaternion.Slerp(_playerCompositionRoot.transform.rotation,
                    _characterNetworkManager.NetworkRotation.Value, _characterNetworkManager.NetworkRotationSmoothTime);
            }
        }

        protected virtual void AddInputActionsCallbacks()
        {
            PlayerControls.Player.Sprint.performed += OnSprintPerformed;
            PlayerControls.Player.Sprint.canceled += OnSprintCanceled;
            PlayerControls.Player.Move.performed += OnMovementPerformed;
            PlayerControls.Player.Move.canceled += OnMovementCanceled;
            PlayerControls.Player.WalkToggle.performed += OnWalkToggleStarted;
            PlayerControls.Player.WalkToggle.canceled += OnWalkToggleCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerControls.Player.Sprint.performed -= OnSprintPerformed;
            PlayerControls.Player.Sprint.canceled -= OnSprintCanceled;
            PlayerControls.Player.Move.performed -= OnMovementPerformed;
            PlayerControls.Player.Move.canceled -= OnMovementCanceled;
            PlayerControls.Player.WalkToggle.performed -= OnWalkToggleStarted;
            PlayerControls.Player.WalkToggle.canceled -= OnWalkToggleCanceled;
        }

        #region МОЖНО ЛИ ЗДЕСЬ ПЕРЕНЕСТИ ЛОГИКУ В КЛАССЫ СОСТОЯНИЙ?
        protected virtual void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig._isButtonHeld = true;  // Устанавливаем флаг удержания кнопки
            _playerConfig.MovementStateConfig._timeButtonHeld = 0f;  // Сбрасываем таймер удержания
        }
        
        protected virtual void OnSprintCanceled(InputAction.CallbackContext context)
        {
            if (_playerConfig.MovementStateConfig._isButtonHeld == false)
            {
                return;
            }

            _playerConfig.MovementStateConfig._isButtonHeld = false;

            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                // Завершаем спринт, если он был активирован
                _playerConfig.MovementStateConfig.ShouldSprint = false;
                // Debug.Log("Спринт завершён");
            }
            else
            {
                if (_playerConfig.MovementStateConfig.IsAirborning) //  =СТРАННО=
                {
                    return;
                }
                
                // Выполняем кувырок, если кнопка была нажата кратковременно
                OnDodgeStarted(context);
            }
        }
        
        protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
        {
            if (Data.MovementInput == Vector2.zero || _playerConfig.MovementStateConfig.IsPerformingStaticAction)
            {
                OnBackStepped(context);
                return;
            }
    
            StateSwitcher.SwitchState<DodgingState>();
            // Debug.Log("Выполнен кувырок");
        }
        
        protected virtual void OnBackStepped(InputAction.CallbackContext context)
        {
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
            {
                return;
            }
    
            if (Data.MovementInput != Vector2.zero)
            {
                return;
            }
    
            StateSwitcher.SwitchState<BackSteppingState>();
        }
        #endregion
        
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig.ShouldWalk = true;
        }
        
        protected virtual void OnWalkToggleCanceled(InputAction.CallbackContext context)
        {
            _playerConfig.MovementStateConfig.ShouldWalk = false;
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
        
        protected virtual void Move()
        {
            // if (Data.MovementInput == Vector2.zero)
            if (Data.MovementInput == Vector2.zero || Data.MovementSpeedModifier == 0f)
            {
                return;
            }

            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                return;
            }
            
            _playerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                _playerConfig.MovementStateConfig._movementDirection * Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime);
        }
        
        protected virtual void HandleVerticalMovement()
        {
            _playerCompositionRoot.CharacterController.Move(
                _playerConfig.MovementStateConfig.YVelocity * Time.deltaTime);
        }
        
        protected virtual void Rotate()
        {
            if (_playerConfig.MovementStateConfig._movementDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_playerConfig.MovementStateConfig._movementDirection);
                
                Quaternion targetRotation = Quaternion.Slerp(
                    PlayerView.transform.rotation,
                    newRotation,
                    _playerConfig.MovementStateConfig.RotationSpeed * Time.deltaTime);
                
                PlayerView.transform.rotation = targetRotation;
            }
        }

        protected virtual void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleCameraRotation();
        }
        
        protected virtual void HandleFollowTarget()
        {
            _playerCameraMovement.FollowTarget(_playerCompositionRoot.PlayerView.transform);
            // _playerCameraMovement.transform.position = Vector3.SmoothDamp(
            //     _playerCameraMovement.transform.position,
            //     _playerCompositionRoot.PlayerView.transform.position,
            //     ref _playerCameraMovement.CameraVelocity,
            //     _playerCameraMovement.CameraSmoothSpeed * Time.deltaTime);
        }
        
        protected virtual void HandleCameraRotation()
        {
            Quaternion playerCameraPivotRotation = _playerCameraMovement.CameraPivotTransform.rotation;
            _playerCameraMovement.PlayerCameraYRotation += Data.CameraHorizontalInput * _playerConfig.MovementStateConfig.Sensitivity;
            _playerCameraMovement.PlayerCameraXRotation -= Data.CameraVerticalInput * _playerConfig.MovementStateConfig.Sensitivity;;
            _playerCameraMovement.PlayerCameraXRotation = Mathf.Clamp(
                _playerCameraMovement.PlayerCameraXRotation, 
                _playerConfig.MovementStateConfig.MinimumPivot, 
                _playerConfig.MovementStateConfig.MaximumPivot);
            
            playerCameraPivotRotation = Quaternion.Euler(
                _playerCameraMovement.PlayerCameraXRotation,
                _playerCameraMovement.PlayerCameraYRotation,
                playerCameraPivotRotation.eulerAngles.z);
            
            _playerCameraMovement.CameraPivotTransform.localRotation = playerCameraPivotRotation;
        }

        #region REUSABLE METHODS
        protected virtual Vector3 GetMovementInputDirection()
        {
            // Получаем правое и переднее направление из положения камеры
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
            
            // Вычисляем направление движения на основе ввода пользователя
            Vector3 movementDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;
            
            movementDirection.y = 0;
            
            movementDirection.Normalize();
            
            return movementDirection;
        }
        #endregion
    }
}