using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public abstract class MovementState : IState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly PlayerCameraMovement _playerCameraMovement;
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        protected readonly PlayerConfig PlayerConfig;
        
        protected MovementState(
            IStateSwitcher stateSwitcher, 
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _playerCameraMovement = playerCompositionRoot.PlayerCameraMovement;
            
            StateSwitcher = stateSwitcher;
            Data = data;
            PlayerConfig = playerCompositionRoot.PlayerConfig;
        }
        
        protected PlayerControls PlayerControls => _playerCompositionRoot.PlayerControls;
        protected PlayerView PlayerView => _playerCompositionRoot.PlayerView;

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
            Move();
            Rotate();
            HandleVerticalMovement();
            CheckButtonHeld();
        }

        public virtual void LateUpdate()
        {
            HandleAllCameraActions();
        }
        
        public virtual void HandleAllInputs()
        {
            // if (_playerCompositionRoot.IsOwner)
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            {
                HandleMovementInput();
                HandleCameraInput();
                
                UpdateAnimatorMovementParameters(0, Data.MoveAmount);   //  LOCOMOTION MOVEMENT WITHOUT TARGET
            }
            // Используем синхронизированные данные для анимаций
            else
            {
                UpdateAnimatorMovementParameters(0, _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value);
            }
        }
        
        private void CheckButtonHeld()
        {
            if (PlayerConfig.MovementStateConfig._isButtonHeld)
            {
                PlayerConfig.MovementStateConfig._timeButtonHeld += Time.deltaTime;

                if (PlayerConfig.MovementStateConfig._timeButtonHeld >= PlayerConfig.MovementStateConfig._holdTimeThreshold)
                {
                    if (!PlayerConfig.MovementStateConfig.ShouldSprint)
                    {
                        PlayerConfig.MovementStateConfig.ShouldSprint = true;
                    }
                }
            }
        }
        
        private void HandleMovementInput()
        {
            // Если персонаж принадлежит игроку, ввод считывается с контроллера
            Data.MovementInput = PlayerControls.Player.Move.ReadValue<Vector2>();

            // Разделение входных данных на вертикальные и горизонтальные составляющие
            Data.VerticalInput = Data.MovementInput.y;
            Data.HorizontalInput = Data.MovementInput.x;

            Data.MoveAmount = Mathf.Clamp01(
                Mathf.Abs(Data.VerticalInput) + Mathf.Abs(Data.HorizontalInput));
            
            if (Data.MoveAmount <= 0.5 && Data.MoveAmount > 0)
            {
                Data.MoveAmount = 0.5f;
                
                PlayerConfig.MovementStateConfig.ShouldWalk = true;
            }
            else if (Data.MoveAmount > 0.5 && Data.MoveAmount <= 1)
            {
                Data.MoveAmount = 1;
                
                PlayerConfig.MovementStateConfig.ShouldWalk = false;
            }
            
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            // if (_playerCompositionRoot.IsOwner)
            {
                // Обновляем значения в сетевом синхронизаторе для передачи другим игрокам
                _playerCompositionRoot.PlayerNetworkSynchronizer.VerticalMovement.Value = Data.VerticalInput;
                _playerCompositionRoot.PlayerNetworkSynchronizer.HorizontalMovement.Value = Data.HorizontalInput;
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = Data.MoveAmount;
            }
            else
            {
                // Если персонаж не принадлежит игроку, данные получаем через сетевой синхронизатор
                Data.VerticalInput = _playerCompositionRoot.PlayerNetworkSynchronizer.VerticalMovement.Value;
                Data.HorizontalInput = _playerCompositionRoot.PlayerNetworkSynchronizer.HorizontalMovement.Value;
                Data.MoveAmount = _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value;
            }
        }

        private void UpdateAnimatorMovementParameters(float horizontalMovementData, float verticalMovementData)
        {
            int vertical = Animator.StringToHash("Vertical");
            int horizontal = Animator.StringToHash("Horizontal");

            if (PlayerConfig.MovementStateConfig.ShouldSprint && Data.MovementInput != Vector2.zero)
            {
                if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
                // if (_playerCompositionRoot.IsOwner)
                {
                    verticalMovementData = 2;
                    _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = verticalMovementData;
                }
                else
                {
                    Data.MoveAmount = verticalMovementData;
                }
            }
            
            PlayerView.Animator.SetFloat(horizontal, horizontalMovementData, 0.1f, Time.deltaTime);
            PlayerView.Animator.SetFloat(vertical, verticalMovementData, 0.1f, Time.deltaTime);
        }
        
        private void HandleCameraInput()
        {
            // Если персонаж принадлежит игроку, ввод считывается локально
            Data.CameraInput = PlayerControls.Player.Look.ReadValue<Vector2>();

            Data.CameraVerticalInput = Data.CameraInput.y;
            Data.CameraHorizontalInput = Data.CameraInput.x;
                       
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            // if (_playerCompositionRoot.IsOwner)
            {
                // Обновляем значения в сетевом синхронизаторе для передачи другим игрокам
                _playerCompositionRoot.PlayerNetworkSynchronizer.CameraVerticalMovement.Value = Data.CameraVerticalInput;
                _playerCompositionRoot.PlayerNetworkSynchronizer.CameraHorizontalMovement.Value = Data.CameraHorizontalInput;
            }
            else
            {
                // Если персонаж не принадлежит игроку, данные получаем через сетевой синхронизатор
                Data.CameraVerticalInput = _playerCompositionRoot.PlayerNetworkSynchronizer.CameraVerticalMovement.Value;
                Data.CameraHorizontalInput = _playerCompositionRoot.PlayerNetworkSynchronizer.CameraHorizontalMovement.Value;
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

        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            PlayerConfig.MovementStateConfig._isButtonHeld = true;
            PlayerConfig.MovementStateConfig._timeButtonHeld = 0f;
        }
        
        protected virtual void OnSprintCanceled(InputAction.CallbackContext context)
        {
            if (PlayerConfig.MovementStateConfig._isButtonHeld == false)
            {
                return;
            }

            PlayerConfig.MovementStateConfig._isButtonHeld = false;

            if (PlayerConfig.MovementStateConfig.ShouldSprint)
            {
                // End the sprint if it was activated
                PlayerConfig.MovementStateConfig.ShouldSprint = false;
                return;
            }

            OnDodgeStarted(context);
        }
        
        protected virtual void OnDodgeStarted(InputAction.CallbackContext context)
        {
            if (Data.MovementInput == Vector2.zero)
            {
                OnBackStepped(context);
                return;
            }
    
            StateSwitcher.SwitchState<DodgingState>();
        }
        
        protected virtual void OnBackStepped(InputAction.CallbackContext context)
        {
            if (Data.MovementInput != Vector2.zero && Data.MovementSpeedModifier != 0)
            {
                return;
            }
    
            StateSwitcher.SwitchState<BackSteppingState>();
        }
        
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            PlayerConfig.MovementStateConfig.ShouldWalk = true;
        }
        
        private void OnWalkToggleCanceled(InputAction.CallbackContext context)
        {
            PlayerConfig.MovementStateConfig.ShouldWalk = false;
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<IdlingState>();
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
        
        protected virtual void Move()
        {
            if (Data.MovementInput == Vector2.zero || Data.MovementSpeedModifier == 0f)
            {
                return;
            }

            if (_playerCompositionRoot.GroundChecker.isTouches == false)
            {
                return;
            }
            
            PlayerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                PlayerConfig.MovementStateConfig._movementDirection * (Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime));
        }
        
        private void HandleVerticalMovement()
        {
            _playerCompositionRoot.CharacterController.Move(
                PlayerConfig.MovementStateConfig.YVelocity * Time.deltaTime);
        }
        
        protected virtual void Rotate()
        {
            if (PlayerConfig.MovementStateConfig._movementDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(PlayerConfig.MovementStateConfig._movementDirection);
                
                Quaternion targetRotation = Quaternion.Slerp(
                    PlayerView.transform.rotation,
                    newRotation,
                    PlayerConfig.MovementStateConfig.RotationSpeed * Time.deltaTime);
                
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
            _playerCameraMovement.FollowTarget(_playerCompositionRoot.PlayerView.transform);
        }
        
        private void HandleCameraRotation()
        {
            Quaternion playerCameraPivotRotation = _playerCameraMovement.CameraPivotTransform.rotation;
            _playerCameraMovement.PlayerCameraYRotation += Data.CameraHorizontalInput * PlayerConfig.MovementStateConfig.Sensitivity;
            _playerCameraMovement.PlayerCameraXRotation -= Data.CameraVerticalInput * PlayerConfig.MovementStateConfig.Sensitivity;
            _playerCameraMovement.PlayerCameraXRotation = Mathf.Clamp(
                _playerCameraMovement.PlayerCameraXRotation, 
                PlayerConfig.MovementStateConfig.MinimumPivot, 
                PlayerConfig.MovementStateConfig.MaximumPivot);
            
            playerCameraPivotRotation = Quaternion.Euler(
                _playerCameraMovement.PlayerCameraXRotation,
                _playerCameraMovement.PlayerCameraYRotation,
                playerCameraPivotRotation.eulerAngles.z);
            
            _playerCameraMovement.CameraPivotTransform.localRotation = playerCameraPivotRotation;
        }

        protected Vector3 GetMovementInputDirection()
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
    }
}