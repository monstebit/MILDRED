using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class BackSteppingState : GroundedState
    {
        private MovementStateConfig _movementStateConfig;
        private BackSteppingStateConfig _backSteppingStateConfig;
        
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
            
            PlayerView.StopBackStepping();
        }
        #endregion
        
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        
            PlayerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
        }
        
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            
            PlayerControls.PlayerMovement.Movement.performed -= OnMovementPerformed;
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
    }
}