using System;
using System.Threading.Tasks;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class BackSteppingState : PerformingActionState
    {
        private MovementStateConfig _movementStateConfig;
        private BackSteppingStateConfig _backSteppingStateConfig;
        private PlayerInputHandler _playerInputHandler;
        
        public BackSteppingState(IStateSwitcher stateSwitcher, 
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
            _movementStateConfig = playerInputHandler.PlayerConfig.MovementStateConfig;
            _backSteppingStateConfig = playerInputHandler.PlayerConfig.BackSteppingStateConfig;
            _playerInputHandler = playerInputHandler;
        }
    
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartBackStepping();
            
            _movementStateConfig.IsPerformingAction = true;
            
            _backSteppingStateConfig._startTime = Time.time;
            
            StartBackStep();
        }

        public override void Update()
        {
            base.Update();
            
            PerformBackStep();
            
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

            // _movementStateConfig.IsPerformingAction = false;
            
            PlayerView.StopBackStepping();
        }
        #endregion
        
        #region BackStep
        public void PerformBackStep()
        {
            // if (_backSteppingStateConfig._lastStepBackDirection != Vector3.zero && 
            //     BackStepStillInProgress() && 
            //     HasNotExceededBackStepDistance())
            if (_backSteppingStateConfig._lastStepBackDirection != Vector3.zero && 
                BackStepStillInProgress())
            {
                MoveCharacterBackward();
            }
            else
            {
                EndBackStep();
            }
        }
        
        public bool BackStepStillInProgress()
        {
            return Time.time < _backSteppingStateConfig._startTime + _backSteppingStateConfig.StepBackDuration;
        }
        
        public void MoveCharacterBackward()
        {
            if (_backSteppingStateConfig._lastStepBackDirection != Vector3.zero)
            {
                BackStepWithDelay();
                // _playerInputHandler.CharacterController.Move(
                //     -_backSteppingStateConfig._lastDodgeDirection * _backSteppingStateConfig.StepBackSpeed * Time.deltaTime);
            }
            else
            {
                EndBackStep();
            }
        }
        
        private async Task BackStepWithDelay()
        {
            await Task.Delay(TimeSpan.FromSeconds(_backSteppingStateConfig.StepBackDelay));
            
            _playerInputHandler.CharacterController.Move(
                -_backSteppingStateConfig._lastStepBackDirection * _backSteppingStateConfig.StepBackSpeed * Time.deltaTime);
        }
        
        // private bool HasNotExceededBackStepDistance()
        // {
        //     return Vector3.Distance(
        //         _backSteppingStateConfig._startStepBackPosition, 
        //         _playerInputHandler.CharacterController.transform.position) < _backSteppingStateConfig.StepBackDistance;
        // }
        
        public void StartBackStep()
        {
            _backSteppingStateConfig._lastStepBackDirection = PlayerView.transform.forward;
            _backSteppingStateConfig._lastStepBackDirection.y = 0;
            _backSteppingStateConfig._lastStepBackDirection.Normalize();

            if (_backSteppingStateConfig._lastStepBackDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(_backSteppingStateConfig._lastStepBackDirection);
                PlayerView.transform.rotation = newRotation;
            } 
            
            _backSteppingStateConfig._startTime = Time.time;
            _backSteppingStateConfig._startStepBackPosition = _playerInputHandler.CharacterController.transform.position;
        }
        
        private void EndBackStep()
        {
            _movementStateConfig.IsPerformingAction = false;
        }
        #endregion
        
        #region COMMENTED CODE
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
        
        // protected override void OnMovementCanceled(InputAction.CallbackContext context)
        // {
        //     Debug.Log("OnMovementCanceled");
        // }

        // private async Task WaitAfterBackStep()
        // {
        //     await Task.Delay(TimeSpan.FromSeconds(2f));
        // }
        #endregion

    }
}