using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class DodgingState : PerformingActionState
    {
        private DodgeStateConfig _dodgeStateConfig;
        private MovementStateConfig _movementStateConfig;
        private PlayerConfig _playerConfig;
        private PlayerInputHandler _playerInputHandler;
        private PlayerCameraMovement _playerCameraMovement;
        
        private int _consecutiveDashedUsed;

        public DodgingState(
            IStateSwitcher stateSwitcher,
            PlayerInputHandler playerInputHandler,
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerPlayerCameraMovement,
            StateMachineData data) : base(
            stateSwitcher,
            playerInputHandler,
            characterNetworkManager,
            playerPlayerCameraMovement,
            data)
            {
                _dodgeStateConfig = playerInputHandler.PlayerConfig.DodgeStateConfig;
                _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
                _playerInputHandler = playerInputHandler;
                _playerCameraMovement = playerPlayerCameraMovement;
            }
         
        #region IState METHODS
        public override void Enter()
        {
            Data.MovementSpeedModifier = _dodgeStateConfig.SpeedModifier;
            
            base.Enter();
            
            PlayerView.StartDodging();
            
            if (_dodgeStateConfig._dodgingTimer <= 0)
            {
                // _dodgeStateConfig._dodgingTimer = 0.4f;
                _dodgeStateConfig._dodgingTimer = 0.2f;
                return;
            }

            _movementStateConfig.IsPerformingAction = true;
            
            _dodgeStateConfig._startTime = Time.time;
            
            StartDodge();
        }

        public override void Update()
        {
            base.Update();
            
            PerformDodge();
                
            if (_movementStateConfig.IsPerformingAction == false)
            {
                if (Data.MovementInput == Vector2.zero)
                {
                    StateSwitcher.SwitchState<IdlingState>();
                    
                    return;
                }

                OnMove();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopDodging();
            
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

            _dodgeStateConfig._lastDodgeDirection = cameraObjectForward * Data.VerticalInput + cameraObjectRight * Data.HorizontalInput;
            _dodgeStateConfig._lastDodgeDirection.y = 0; // Убираем вертикальный компонент
            _dodgeStateConfig._lastDodgeDirection.Normalize();

            if (_dodgeStateConfig._lastDodgeDirection != Vector3.zero)
            {
                // Устанавливаем начальное вращение персонажа в направлении кувырка
                Quaternion newRotation = Quaternion.LookRotation(_dodgeStateConfig._lastDodgeDirection);
                PlayerView.transform.rotation = newRotation;
            }

            _dodgeStateConfig._startTime = Time.time;
            _dodgeStateConfig._startDodgePosition = _playerInputHandler.CharacterController.transform.position;
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
            if (_dodgeStateConfig._lastDodgeDirection != Vector3.zero)
            {
                _playerInputHandler.CharacterController.Move(
                    _dodgeStateConfig._lastDodgeDirection * _dodgeStateConfig._dodgeSpeed * Time.deltaTime);
            }
            else
            {
                // Если нет сохраненного направления, завершаем кувырок
                EndDodge();
            }
        }
        
        private void DodgingTimer()
        {
            if (_dodgeStateConfig._dodgingTimer >= 0)
            {
                _dodgeStateConfig._dodgingTimer -= Time.deltaTime;
            }
        }
        #endregion
        
        #region COMMENTED CODE
        // public override void OnAnimationExitEvent() //  ТРИГГЕР ЗАВЕРШЕНИЯ АНИМАЦИИ
        // {
        //     base.OnAnimationExitEvent();
        // }

        // protected override void AddInputActionsCallbacks()
        // {
        //     base.AddInputActionsCallbacks();
        //
        //     PlayerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
        // }
        //
        // protected override void RemoveInputActionsCallbacks()
        // {
        //     base.RemoveInputActionsCallbacks();
        //     
        //     PlayerControls.PlayerMovement.Movement.performed -= OnMovementPerformed;
        // }
        //
        // protected override void OnMovementCanceled(InputAction.CallbackContext context)
        // {
        // }
        #endregion
    }
}