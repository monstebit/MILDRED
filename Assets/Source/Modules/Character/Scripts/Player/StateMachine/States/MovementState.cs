using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public abstract class MovementState : IState
    {
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        
        protected bool IsIdling() => Data.MoveAmount == 0;
        protected bool IsWalking() => Data.MoveAmount > 0 && Data.MoveAmount <= 0.5f;
        protected bool IsRunning() => Data.MoveAmount > 0.5f;
        protected bool IsDodging() => _movementStateConfig.isDodging;   //  TEST
        
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

        public virtual void Enter()
        {
            // Debug.Log(GetType());   //  ВЫВОД ТИПА НАСЛЕДНИКА (В КАКОМ STATE МЫ СЕЙЧАС НАХОДИМСЯ)
            AddInputActionsCallbacks();
            
            _playerCameraMovement.PlayerCameraYRotation = Data.SavedLeftAndRightLookAngle;
            _playerCameraMovement.PlayerCameraXRotation = Data.SavedUpAndDownLookAngle;
            
            //
            PlayerView.StartMovement();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();

            Data.SavedLeftAndRightLookAngle = _playerCameraMovement.PlayerCameraYRotation;
            Data.SavedUpAndDownLookAngle = _playerCameraMovement.PlayerCameraXRotation;
            
            //
            PlayerView.StopMovement();
        }

        public virtual void HandleInput()
        {
            Data.MovementInput = ReadMovementInput();
            
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
            
            Data.CameraInput = ReadCameraInput();
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
        
        public virtual void Update()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            HandleAllMovement();
        }

        public virtual void LateUpdate()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            HandleAllCameraActions();
        }

        protected virtual void AddInputActionsCallbacks() { }

        protected virtual void RemoveInputActionsCallbacks() { }
        
        private Vector2 ReadMovementInput() => PlayerControls.PlayerMovement.Movement.ReadValue<Vector2>();
        private Vector2 ReadCameraInput() => PlayerControls.PlayerCamera.Movement.ReadValue<Vector2>();
        
        private void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleCameraRotation();
        }
        
        private void HandleFollowTarget()
        {
            _playerCameraMovement.transform.position = Vector3.SmoothDamp(
                _playerCameraMovement.transform.position,   //  КОРРЕКТНАЯ РАБОТА С ApplyRootMotion
                // _playerInputHandler.transform.position,
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

        private void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        
        //  ВАРИАНТ БЕЗ ПЕРЕКАТА
        // private void HandleGroundedMovement()
        // {
        //     Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
        //     Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
        //     
        //     _movementStateConfig.MoveDirection = forward * Data.VerticalInput + right * Data.HorizontalInput;
        //     // _moveDirection.y = 0;
        //     _movementStateConfig.MoveDirection.y = Data.YVelocity;  //  ПРЫЖОК
        //     _movementStateConfig.MoveDirection.Normalize();
        //
        //     _playerInputHandler.CharacterController.Move(_movementStateConfig.MoveDirection * Data.Speed * Time.deltaTime);
        // }
        
        private void HandleGroundedMovement()
        {
            Vector3 forward = _playerCameraMovement.CameraPivotTransform.forward;
            Vector3 right = _playerCameraMovement.CameraPivotTransform.right;
            
            //   МЫ НЕ МОЖЕМ ПЕРЕДВИГАТЬСЯ ПРИ ПЕРЕКАТЕ
            if (IsDodging())
            {
                Vector3 dodgeDirection = _movementStateConfig.MoveDirection * _movementStateConfig.RollDistance; // толкаем на 2 метра в направлении движения
                _playerInputHandler.CharacterController.Move(dodgeDirection * Time.deltaTime);
                return;
            }
            
            _movementStateConfig.MoveDirection = forward * Data.VerticalInput + right * Data.HorizontalInput;
            // _moveDirection.y = 0;
            _movementStateConfig.MoveDirection.y = Data.YVelocity;  //  ПРЫЖОК
            _movementStateConfig.MoveDirection.Normalize();
        
            _playerInputHandler.CharacterController.Move(_movementStateConfig.MoveDirection * Data.Speed * Time.deltaTime);
        }
        
        //  ПОВОРОТ ИГРОКА ПО НАПРАВЛЕНИЮ КАМЕРЫ
        private void HandleRotation()
        {
            if (IsDodging())
                return;
            
            Transform cameraObjectTransform = _playerCameraMovement.CameraObject.transform;

            Vector3 cameraObjectForward = cameraObjectTransform.forward;
            Vector3 cameraObjectRight = cameraObjectTransform.right;

            _movementStateConfig.TargetRotationDirection = cameraObjectForward * Data.VerticalInput;
            _movementStateConfig.TargetRotationDirection = _movementStateConfig.TargetRotationDirection + cameraObjectRight * Data.HorizontalInput;
            _movementStateConfig.TargetRotationDirection.y = 0;
            _movementStateConfig.TargetRotationDirection.Normalize();

            if (_movementStateConfig.TargetRotationDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_movementStateConfig.TargetRotationDirection);
                
                Quaternion targetRotation = Quaternion.Slerp(
                    PlayerView.transform.rotation,
                    newRotation,
                    _movementStateConfig.RotationSpeed * Time.deltaTime);
                
                PlayerView.transform.rotation = targetRotation;
            }
        }
    }
}