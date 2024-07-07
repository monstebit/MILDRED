using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public abstract class MovementState : IState
    {
        //  JUMP TEST
        private Vector3 jumpDirection = Vector3.zero;
        public float speed = 5.0f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        private bool isGrounded;
        
        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        private float sensitivity = 1.5f;
        private float yOffset = 1.5f;
        private float _leftAndRightRotationSpeed = 220;
        private float _upAndDownRotationSpeed = 220;
        private float _minimumPivot = -30;
        private float _maximumPivot = 80;
        private float _rotationSpeed = 15;
        
        private Vector3 _cameraVelocity = Vector3.zero;
        private Vector3 _cameraObjectPosition;
        private float playerCameraXRotation;
        private float playerCameraYRotation;
        private float _cameraSmoothSpeed = 0.125f;
        private float _cameraZPosition;
        private float _targetCameraZPosition;
        
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly StateMachineData Data;
        protected bool IsPlayerIdling() => Data.MoveAmount == 0;
        protected bool IsPlayerWalking() => Data.MoveAmount > 0 && Data.MoveAmount <= 0.5f;
        protected bool IsPlayerSprinting() => Data.MoveAmount > 0.5f;
        
        private readonly PlayerInputHandler _playerInputHandler;
        private CharacterNetworkManager _characterNetworkManager;
        private CameraMovement _cameraMovement;
        
        public MovementState(
            IStateSwitcher stateSwitcher, 
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement,
            StateMachineData data)
        {
            StateSwitcher = stateSwitcher;
            _playerInputHandler = playerInputHandler;
            _characterNetworkManager = characterNetworkManager;
            _cameraMovement = cameraMovement;
            Data = data;
        }
        
        protected PlayerControls PlayerControls => _playerInputHandler.PlayerControls;
        protected PlayerView PlayerView => _playerInputHandler.PlayerView;

        public virtual void Enter()
        {
            Debug.Log(GetType());   //  ВЫВОД ТИПА НАСЛЕДНИКА (В КАКОМ STATE МЫ СЕЙЧАС НАХОДИМСЯ)
            AddInputActionsCallbacks();
            
            playerCameraYRotation = Data.SavedLeftAndRightLookAngle;
            playerCameraXRotation = Data.SavedUpAndDownLookAngle;
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();

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
            HandleJumpingMovement(); //  JUMP TEST
        }

        //  JUMP TEST
        private void HandleJumpingMovement()
        {
            if (_playerInputHandler.GroundChecker.isTouches)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    jumpDirection.y = jumpSpeed;
                }
            }
            
            jumpDirection.y -= gravity * Time.deltaTime;    // Применение гравитации всегда
            _playerInputHandler.CharacterController.Move(jumpDirection * Time.deltaTime);  // Перемещение персонажа
        }

        public virtual void LateUpdate()
        {
            if (!_playerInputHandler.IsOwner)
                return;
            
            HandleAllCameraActions();
        }

        protected virtual void AddInputActionsCallbacks()
        {
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
        }
        
        private Vector2 ReadMovementInput() => PlayerControls.PlayerMovement.Movement.ReadValue<Vector2>();
        private Vector2 ReadCameraInput() => PlayerControls.PlayerCamera.Movement.ReadValue<Vector2>();
        
        private void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleCameraRotation();
        }
        
        private void HandleFollowTarget()
        {
            _cameraMovement.transform.position = Vector3.SmoothDamp(
                _cameraMovement.transform.position,
                _playerInputHandler.transform.position,
                ref _cameraVelocity,
                _cameraSmoothSpeed * Time.deltaTime);
        }
        
        private void HandleCameraRotation()
        {
            Quaternion playerCameraPivotRotation = _cameraMovement.cameraPivotTransform.rotation;
            playerCameraYRotation += Data.CameraHorizontalInput * sensitivity;
            playerCameraXRotation -= Data.CameraVerticalInput * sensitivity;
            playerCameraXRotation = Mathf.Clamp(playerCameraXRotation, _minimumPivot, _maximumPivot);
            
            playerCameraPivotRotation = Quaternion.Euler(
                playerCameraXRotation,
                playerCameraYRotation,
                playerCameraPivotRotation.eulerAngles.z);
            
            _cameraMovement.cameraPivotTransform.localRotation = playerCameraPivotRotation;
        }

        private void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        
        private void HandleGroundedMovement()
        {
            Vector3 forward = _cameraMovement.cameraPivotTransform.forward;
            Vector3 right = _cameraMovement.cameraPivotTransform.right;
            
            _moveDirection = forward * Data.VerticalInput + right * Data.HorizontalInput;
            _moveDirection.y = 0;
            _moveDirection.Normalize();

            _playerInputHandler.CharacterController.Move(_moveDirection * Data.Speed * Time.deltaTime);
        }
        
        private void HandleRotation()
        {
            Transform cameraObjectTransform = _cameraMovement.CameraObject.transform;

            Vector3 cameraObjectForward = cameraObjectTransform.forward;
            Vector3 cameraObjectRight = cameraObjectTransform.right;

            _targetRotationDirection = cameraObjectForward * Data.VerticalInput;
            _targetRotationDirection = _targetRotationDirection + cameraObjectRight * Data.HorizontalInput;
            _targetRotationDirection.y = 0;
            _targetRotationDirection.Normalize();

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