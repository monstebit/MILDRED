using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;

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
            
            _backSteppingStateConfig.IsBackStepping = true;
            
            Keyframe LastFrame = _backSteppingStateConfig.BackStepCurve[_backSteppingStateConfig.BackStepCurve.length - 1];
            
            _backSteppingStateConfig.BackStepTimer = LastFrame.time;
            
            _backSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
            
            _backSteppingStateConfig.LastStepDirection.y = 0;
            
            _backSteppingStateConfig.LastStepDirection.Normalize();
            
            _movementStateConfig.IsPerformingAction = true;
        }

        public override void Update()
        {
            base.Update();
            
            if (_backSteppingStateConfig.IsBackStepping)
            {
                _backSteppingStateConfig.Timer += Time.deltaTime;

                if (_backSteppingStateConfig.Timer < _backSteppingStateConfig.BackStepTimer)
                {
                    float speed = 
                        _backSteppingStateConfig.BackStepCurve.Evaluate(_backSteppingStateConfig.Timer);

                    _playerInputHandler.CharacterController.Move(
                        -_backSteppingStateConfig.LastStepDirection * speed * Time.deltaTime);
                }
                else
                {
                    _movementStateConfig.IsPerformingAction = false;
                    _backSteppingStateConfig.IsBackStepping = false;
                    _backSteppingStateConfig.Timer = 0;
                }
            }
            
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
            
            PlayerView.StopBackStepping();

            _backSteppingStateConfig.Timer = 0f;
            
            _movementStateConfig.IsPerformingAction = false;
            _backSteppingStateConfig.IsBackStepping = false;
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
        #endregion
    }
}