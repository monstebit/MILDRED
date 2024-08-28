using System;
using System.Threading.Tasks;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class BackSteppingState : PerformingActionState
    {
        //
        private Vector3 dodgeDirection;
        private float waitTimer;
        
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
            
            
            
            
            _backSteppingStateConfig.isDodging = true;
            Keyframe LastFrame = _backSteppingStateConfig.DodgeCurve[_backSteppingStateConfig.DodgeCurve.length - 1];
            _backSteppingStateConfig.DodgeTimer = LastFrame.time;
            
            _backSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
            _backSteppingStateConfig.LastStepDirection.y = 0;
            _backSteppingStateConfig.LastStepDirection.Normalize();

            waitTimer = Time.time;;
            
            
            
            
            
            // _backSteppingStateConfig.StartTime = Time.time;
            
            _movementStateConfig.IsPerformingAction = true;
            
            // StartBackStep();
        }

        public override void Update()
        {
            base.Update();
            
            // PerformBackStep();
            
            
            
            
            
            if (_backSteppingStateConfig.isDodging)
            {
                _backSteppingStateConfig.timer += Time.deltaTime;

                if (_backSteppingStateConfig.timer < _backSteppingStateConfig.DodgeTimer)
                {
                    float speed = 
                        _backSteppingStateConfig.DodgeCurve.Evaluate(_backSteppingStateConfig.timer);

                    _playerInputHandler.CharacterController.Move(
                        // -_backSteppingStateConfig.LastStepDirection * Time.deltaTime);
                        -_backSteppingStateConfig.LastStepDirection * speed * Time.deltaTime);
                    // -_backSteppingStateConfig.LastStepDirection * 5 * Time.deltaTime);
                }
                else
                {
                    _movementStateConfig.IsPerformingAction = false;
                    
                    _backSteppingStateConfig.isDodging = false;
                    _backSteppingStateConfig.timer = 0;
                }
            }

            
            
            
            
            
            if (_movementStateConfig.IsPerformingAction == false) 
                // && waitTimer == _backSteppingStateConfig.DodgeTimer + 1f )
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
            
            PlayerView.StopBackStepping();

            _backSteppingStateConfig.timer = 0f;
            
            _movementStateConfig.IsPerformingAction = false;
            _backSteppingStateConfig.isDodging = false;
        }
        #endregion
        
        #region BackStep
        // public void PerformBackStep()
        // {
        //     if (IsValidBackStep() && IsBackStepInProgress() && IsWithinBackStepDistance())
        //     {
        //         ContinueBackStep();
        //     }
        //     else
        //     {
        //         CompleteBackStep();
        //     }
        // }
        //
        // private bool IsValidBackStep()
        // {
        //     return _backSteppingStateConfig.LastStepDirection != Vector3.zero;
        // }
        //
        // private bool IsBackStepInProgress()
        // {
        //     return Time.time < _backSteppingStateConfig.StartTime + _backSteppingStateConfig.BackStepDuration;
        // }
        //
        // private bool IsWithinBackStepDistance()
        // {
        //     return Vector3.Distance(_backSteppingStateConfig.StartStepPosition, 
        //                _playerInputHandler.CharacterController.transform.position) 
        //            < _backSteppingStateConfig.BackStepDistance;
        // }
        //
        // private void ContinueBackStep()
        // {
        //     if (IsValidBackStep())
        //     {
        //         PerformDelayedBackStep();
        //     }
        //     else
        //     {
        //         CompleteBackStep();
        //     }
        // }
        //
        // private async Task PerformDelayedBackStep()
        // {
        //     await Task.Delay(TimeSpan.FromSeconds(_backSteppingStateConfig.BackStepDelay));
        //
        //     _playerInputHandler.CharacterController.Move(
        //         -_backSteppingStateConfig.LastStepDirection * _backSteppingStateConfig.BackStepSpeed * Time.deltaTime);
        // }
        //
        // public void StartBackStep()
        // {
        //     _backSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
        //     _backSteppingStateConfig.LastStepDirection.y = 0;
        //     _backSteppingStateConfig.LastStepDirection.Normalize();
        //
        //     if (IsValidBackStep())
        //     {
        //         Quaternion newRotation = Quaternion.LookRotation(_backSteppingStateConfig.LastStepDirection);
        //         PlayerView.transform.rotation = newRotation;
        //     }
        //
        //     _backSteppingStateConfig.StartTime = Time.time;
        //     _backSteppingStateConfig.StartStepPosition = _playerInputHandler.CharacterController.transform.position;
        // }
        //
        // private void CompleteBackStep()
        // {
        //     _movementStateConfig.IsPerformingAction = false;
        // }
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