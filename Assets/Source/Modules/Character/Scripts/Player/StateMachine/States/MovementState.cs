using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public abstract class MovementState : IState
    {
        private float sensitivity = 1.5f;
        private float yOffset = 1.5f;
        private float playerCameraXRotation;
        private float playerCameraYRotation;
        //  CAMERA
        private float _leftAndRightRotationSpeed = 220;
        private float _upAndDownRotationSpeed = 220;
        private float _minimumPivot = -30;
        private float _maximumPivot = 80;
        // private float _cameraSmoothSpeed = 1;
        private float _cameraSmoothSpeed = 0.125f;
        private Vector3 _cameraVelocity = Vector3.zero;
        private Vector3 _cameraObjectPosition;
        private float _cameraZPosition;
        private float _targetCameraZPosition;
        // private float _cameraObjectPositionZInterpolation = 0.2f;
        //  MOVEMENT
        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        private float _rotationSpeed = 15;
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        
        private readonly PlayerInputHandler _playerInputHandler;
        private CharacterNetworkManager _characterNetworkManager;
        
        // public MovementState(IStateSwitcher stateSwitcher, PlayerInputManager playerInputManager, StateMachineData data)
        public MovementState(IStateSwitcher stateSwitcher, PlayerInputHandler playerInputHandler, CharacterNetworkManager characterNetworkManager,StateMachineData data)
        {
            StateSwitcher = stateSwitcher;
            _playerInputHandler = playerInputHandler;
            _characterNetworkManager = characterNetworkManager;
            Data = data;
        }
        
        protected PlayerControls PlayerControls => _playerInputHandler.PlayerControls;
        protected CharacterController CharacterController => _playerInputHandler.CharacterController;
        protected PlayerView PlayerView => _playerInputHandler.PlayerView;

        public virtual void Enter()
        {
            //  ВЫВОД ТИПА НАСЛЕДНИКА (В КАКОМ STATE МЫ СЕЙЧАС НАХОДИМСЯ)
            // Debug.Log(GetType());
            
            playerCameraYRotation = Data.SavedLeftAndRightLookAngle;
            playerCameraXRotation = Data.SavedUpAndDownLookAngle;
        }

        public virtual void Exit()
        {
            Data.SavedLeftAndRightLookAngle = playerCameraYRotation;
            Data.SavedUpAndDownLookAngle = playerCameraXRotation;
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
            
            //
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
        
        protected bool IsPlayerWalking() => Data.MoveAmount > 0 && Data.MoveAmount <= 0.5f;
        
        protected bool IsPlayerSprinting() => Data.MoveAmount > 0.5f;
        
        private Vector2 ReadMovementInput() => PlayerControls.PlayerMovement.Movement.ReadValue<Vector2>();
        
        private Vector2 ReadCameraInput() => PlayerControls.PlayerCamera.Movement.ReadValue<Vector2>();
        
        //  КАМЕРА
        public void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleCameraRotation();
        }
        
        private void HandleFollowTarget()
        {
            CameraMovement.instance.transform.position = Vector3.SmoothDamp(
                CameraMovement.instance.transform.position,
                _playerInputHandler.transform.position,
                ref _cameraVelocity,
                _cameraSmoothSpeed * Time.deltaTime);
        }
        
        private void HandleCameraRotation()
        {
            Quaternion playerCameraPivotRotation = CameraMovement.instance.cameraPivotTransform.rotation;
            playerCameraYRotation += Data.CameraHorizontalInput * sensitivity;
            playerCameraXRotation -= Data.CameraVerticalInput * sensitivity;
            playerCameraXRotation = Mathf.Clamp(playerCameraXRotation, _minimumPivot, _maximumPivot);
            
            playerCameraPivotRotation = Quaternion.Euler(
                playerCameraXRotation,
                playerCameraYRotation,
                playerCameraPivotRotation.eulerAngles.z);
            
            // CameraMovement.instance.cameraPivotTransform.rotation = playerCameraPivotRotation;
            CameraMovement.instance.cameraPivotTransform.localRotation = playerCameraPivotRotation;
        }

        //  PLAYER MOVEMENT
        public void HandleAllMovement()
        {
            //  ДОБАВИЛ ПРОВЕРКУ НА INPUT
            // if (IsMovementInputZero())
            //     return;
            
            HandleGroundedMovement();
            HandleRotation();
        }
        
        private void HandleGroundedMovement()
        {
            // Получаем направление камеры
            // Рассчитываем forward и right направления камеры
            Vector3 forward = CameraMovement.instance.cameraPivotTransform.forward;
            Vector3 right = CameraMovement.instance.cameraPivotTransform.right;
            
            // Рассчитываем направление движения на основе ввода и направлений камеры
            _moveDirection = forward * Data.VerticalInput + right * Data.HorizontalInput;
            
            // Убираем влияние высоты
            _moveDirection.y = 0;
            // Нормализация вектора направления
            _moveDirection.Normalize();

            // Перемещение персонажа
            _playerInputHandler.CharacterController.Move(_moveDirection * Data.Speed * Time.deltaTime);
            
            Debug.DrawRay(CameraMovement.instance.cameraPivotTransform.position, _moveDirection, Color.blue);
        }
        
        private void HandleRotation()
        {
            Transform cameraObjectTransform = CameraMovement.instance.CameraObject.transform;

            Vector3 cameraObjectForward = cameraObjectTransform.forward;
            Vector3 cameraObjectRight = cameraObjectTransform.right;

            _targetRotationDirection = cameraObjectForward * Data.VerticalInput;
            _targetRotationDirection = _targetRotationDirection + cameraObjectRight * Data.HorizontalInput;
            _targetRotationDirection.y = 0; // Убираем влияние высоты
            _targetRotationDirection.Normalize(); // Нормализуем вектор

            // Проверка на ненулевое направление
            if (_targetRotationDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
                
                Quaternion targetRotation = Quaternion.Slerp(
                    PlayerView.transform.rotation,
                    newRotation,
                    _rotationSpeed * Time.deltaTime);
                
                PlayerView.transform.rotation = targetRotation;
            }
        }
    }
}