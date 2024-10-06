using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public abstract class MovementState : IState
    {
        private readonly PlayerCompositionRoot _playerCompositionRoot;
        private readonly PlayerCameraMovement _playerCameraMovement;

        private bool IsOwner => _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner; 
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        protected PlayerConfig PlayerConfig;
        
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
            // Debug.Log($"State: {GetType().Name}");
            // Debug.Log($"Speed Modifier: {Data.JumpModifier}");
            // Debug.Log($"{_playerCompositionRoot.PlayerInput.currentControlScheme}");
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
            HandleMovementInput();
            HandleCameraInput();
            
            HandleMovementByControlScheme();//
            SyncControlScheme();//
        }
        
        private void HandleMovementInput()
        {
            var stringScheme = _playerCompositionRoot.PlayerInput.currentControlScheme;
            if (stringScheme == "Gamepad")
            {
                Data.ControlScheme = 1;
            }
            else if (stringScheme == "KeyboardAndMouse")
            {
                Data.ControlScheme = 2;
            }
            
            Data.MovementInput = PlayerControls.Player.Move.ReadValue<Vector2>();
            
            Data.VerticalInput = Data.MovementInput.y;
            Data.HorizontalInput = Data.MovementInput.x;
            _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = 
                Mathf.Clamp01(Mathf.Abs(Data.VerticalInput) + Mathf.Abs(Data.HorizontalInput));

            if (Data.MovementInput == Vector2.zero) return;

            HandleSprinting();
            HandleMovementByControlScheme();
        }
        
        private void HandleSprinting()
        {
            if (PlayerConfig.MovementStateConfig.ShouldSprint &&
                PlayerConfig.MovementStateConfig._timeButtonHeld >= PlayerConfig.MovementStateConfig._holdTimeThreshold)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = 2f;
            }
        }
        
        private void HandleMovementByControlScheme()
        {
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.ControlScheme.Value == 1)
            {
                HandleGamepadMovement();
            }
            else if (_playerCompositionRoot.PlayerNetworkSynchronizer.ControlScheme.Value == 2)
            {
                HandleKeyboardMovement();
            }
        }
        
        private void HandleGamepadMovement()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                PlayerConfig.MovementStateConfig.ShouldWalk = false;
                return;
            }
            
            if (_playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value <= 0.5f && _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value > 0f)
            {
                PlayerConfig.MovementStateConfig.ShouldWalk = true;
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = 0.5f;
            }
            else if (_playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value > 0.5f && _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value <= 1f)
            {
                PlayerConfig.MovementStateConfig.ShouldWalk = false;
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = 1f;
            }
        }

        private void HandleKeyboardMovement()
        {
            if (Data.MovementInput == Vector2.zero) return;
            
            if (PlayerConfig.MovementStateConfig.ShouldWalk &&
                PlayerConfig.MovementStateConfig.ShouldSprint == false)
            {
                _playerCompositionRoot.PlayerNetworkSynchronizer.MoveAmount.Value = 0.5f;
            }
        }
        
        private void HandleCameraInput()
        {
            Data.CameraInput = PlayerControls.Player.Look.ReadValue<Vector2>();

            Data.CameraVerticalInput = Data.CameraInput.y;
            Data.CameraHorizontalInput = Data.CameraInput.x;
        }
        
        private void SyncControlScheme()
        {
            _playerCompositionRoot.PlayerNetworkSynchronizer.ControlScheme.Value = Data.ControlScheme;
            // if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            // {
            //     _playerCompositionRoot.PlayerNetworkSynchronizer.ControlScheme.Value = Data.ControlScheme;
            // }
        }
        
        private void CheckButtonHeld()
        {
            if (PlayerConfig.MovementStateConfig._isButtonHeld == false) return;
            
            PlayerConfig.MovementStateConfig._timeButtonHeld += Time.deltaTime;

            if (PlayerConfig.MovementStateConfig._timeButtonHeld >=
                PlayerConfig.MovementStateConfig._holdTimeThreshold == false) return;
            
            if (PlayerConfig.MovementStateConfig.ShouldSprint == false)
            {
                PlayerConfig.MovementStateConfig.ShouldSprint = true;
            }
        }
        
        protected Vector3 GetMovementInputDirection()
        {
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
            
            Vector3 movementDirection = forward * Data.MovementInput.y + right * Data.MovementInput.x;
            movementDirection.y = 0;
            movementDirection.Normalize();
            return movementDirection;
        }
        
        protected virtual void Move()
        {
            if (Data.MovementInput == Vector2.zero || Data.MovementSpeedModifier == 0f) return;

            if (_playerCompositionRoot.GroundChecker.isTouches == false) return;
            
            PlayerConfig.MovementStateConfig._movementDirection = GetMovementInputDirection();
            
            _playerCompositionRoot.CharacterController.Move(
                PlayerConfig.MovementStateConfig._movementDirection * (Data.BaseSpeed * Data.MovementSpeedModifier * Time.deltaTime));
        }
        
        protected virtual void Rotate()
        {
            if (PlayerConfig.MovementStateConfig._movementDirection == Vector3.zero) return;
            
            Quaternion newRotation = Quaternion.LookRotation(PlayerConfig.MovementStateConfig._movementDirection);
                
            Quaternion targetRotation = Quaternion.Slerp(
                PlayerView.transform.rotation,
                newRotation,
                PlayerConfig.MovementStateConfig.RotationSpeed * Time.deltaTime);
                
            PlayerView.transform.rotation = targetRotation;
        }

        private void HandleVerticalMovement()
        {
            _playerCompositionRoot.CharacterController.Move(
                PlayerConfig.MovementStateConfig.YVelocity * Time.deltaTime);
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
        
        protected virtual void OnMove()
        {
            if (PlayerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                return;
            }
            
            if (PlayerConfig.MovementStateConfig.ShouldWalk)
            {
                StateSwitcher.SwitchState<WalkingState>();
                return;
            }
            
            StateSwitcher.SwitchState<RunningState>();
        }
        
        protected virtual void AddInputActionsCallbacks()
        {
            PlayerControls.Player.StaticAction.performed += OnStaticActionPerformed;
            PlayerControls.Player.StaticAction.canceled += OnStaticActionCanceled;
            PlayerControls.Player.Move.performed += OnMovementPerformed;
            PlayerControls.Player.Move.canceled += OnMovementCanceled;
            PlayerControls.Player.WalkToggle.performed += OnWalkTogglePerformed;
            PlayerControls.Player.WalkToggle.canceled += OnWalkToggleCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerControls.Player.StaticAction.performed -= OnStaticActionPerformed;
            PlayerControls.Player.StaticAction.canceled -= OnStaticActionCanceled;
            PlayerControls.Player.Move.performed -= OnMovementPerformed;
            PlayerControls.Player.Move.canceled -= OnMovementCanceled;
            PlayerControls.Player.WalkToggle.performed -= OnWalkTogglePerformed;
            PlayerControls.Player.WalkToggle.canceled -= OnWalkToggleCanceled;
        }

        protected virtual bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
        
        protected virtual void OnStaticActionPerformed(InputAction.CallbackContext context)
        {
            PlayerConfig.MovementStateConfig._isButtonHeld = true;

            OnBackStepped(context);
        }
        
        protected virtual void OnStaticActionCanceled(InputAction.CallbackContext context)
        {
            if (PlayerConfig.MovementStateConfig._isButtonHeld == false) return;

            PlayerConfig.MovementStateConfig._isButtonHeld = false;

            if (PlayerConfig.MovementStateConfig.ShouldSprint)
            {
                PlayerConfig.MovementStateConfig.ShouldSprint = false;
                PlayerConfig.MovementStateConfig._timeButtonHeld = 0f;
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
            if (Data.MovementInput != Vector2.zero && Data.MovementSpeedModifier != 0) return;
  
            StateSwitcher.SwitchState<BackSteppingState>();
        }

        protected virtual void OnWalkTogglePerformed(InputAction.CallbackContext context)
        {
            PlayerConfig.MovementStateConfig.ShouldWalk = true;
        }
        
        protected virtual void OnWalkToggleCanceled(InputAction.CallbackContext context)
        {
            PlayerConfig.MovementStateConfig.ShouldWalk = false;
        }
        
        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            StateSwitcher.SwitchState<IdlingState>();
        }
        
        #region ANIMATION EVENTS METHODS
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
    }
}