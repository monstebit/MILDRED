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
            base.Enter();
            
            PlayerView.StartDodging();
            
            _dodgeStateConfig.IsDodging = true;
            
            Keyframe LastFrame = _dodgeStateConfig.DodgeCurve[_dodgeStateConfig.DodgeCurve.length - 1];
            
            _dodgeStateConfig.DodgeTimer = LastFrame.time;
            
            _dodgeStateConfig.LastDodgeDirection = PlayerView.transform.forward;
            
            _dodgeStateConfig.LastDodgeDirection.y = 0;
            
            _dodgeStateConfig.LastDodgeDirection.Normalize();
            
            _movementStateConfig.IsPerformingAction = true;
        }

        public override void Update()
        {
            base.Update();
            
            if (_dodgeStateConfig.IsDodging)
            {
                _dodgeStateConfig.Timer += Time.deltaTime;

                if (_dodgeStateConfig.Timer < _dodgeStateConfig.DodgeTimer)
                {
                    float speed = 
                        _dodgeStateConfig.DodgeCurve.Evaluate(_dodgeStateConfig.Timer);

                    _playerInputHandler.CharacterController.Move(
                        _dodgeStateConfig.LastDodgeDirection * speed * Time.deltaTime);
                }
                else
                {
                    _movementStateConfig.IsPerformingAction = false;
                    
                    _dodgeStateConfig.IsDodging = false;
                    
                    _dodgeStateConfig.Timer = 0f;
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
            
            PlayerView.StopDodging();
            
            _dodgeStateConfig.Timer = 0f;
            
            _movementStateConfig.IsPerformingAction = false;
            _dodgeStateConfig.IsDodging = false;
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