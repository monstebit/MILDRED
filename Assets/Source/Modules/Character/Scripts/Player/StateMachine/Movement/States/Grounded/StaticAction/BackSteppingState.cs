using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class BackSteppingState : StaticActionState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        
        public BackSteppingState(
            IStateSwitcher stateSwitcher,
            PlayerCompositionRoot playerCompositionRoot, 
            StateMachineData data) : base(
            stateSwitcher,
            playerCompositionRoot,
            data)
        {
            _playerCompositionRoot = playerCompositionRoot;
            _playerConfig = playerCompositionRoot.PlayerConfig;
        }
    
        #region IState METHODS
        public override void Enter()
        {
            base.Enter();
            
            PlayerView.StartBackStepping();
            
            Keyframe LastFrame = _playerConfig.BackSteppingStateConfig.BackStepCurve[_playerConfig.BackSteppingStateConfig.BackStepCurve.length - 1];
            _playerConfig.BackSteppingStateConfig.BackStepTimer = LastFrame.time;
            _playerConfig.BackSteppingStateConfig.LastStepDirection = PlayerView.transform.forward;
            _playerConfig.BackSteppingStateConfig.LastStepDirection.y = 0;
            _playerConfig.BackSteppingStateConfig.LastStepDirection.Normalize();
        }

        public override void Update()
        {
            base.Update();
            
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction)
            {
                _playerConfig.BackSteppingStateConfig.Timer += Time.deltaTime;

                if (_playerConfig.BackSteppingStateConfig.Timer < _playerConfig.BackSteppingStateConfig.BackStepTimer)
                {
                    float speed = 
                        _playerConfig.BackSteppingStateConfig.BackStepCurve.Evaluate(_playerConfig.BackSteppingStateConfig.Timer);

                    _playerCompositionRoot.CharacterController.Move(
                        -_playerConfig.BackSteppingStateConfig.LastStepDirection * speed * Time.deltaTime);
                }
                else
                {
                    Exit();
                    // _playerConfig.MovementStateConfig.IsPerformingStaticAction = false;
                    // _playerConfig.BackSteppingStateConfig.Timer = 0;
                }
            }
            
            if (_playerConfig.MovementStateConfig.IsPerformingStaticAction == false) 
            {
                //  ON TESTING
                if (_playerCompositionRoot.GroundChecker.isTouches == false)
                {
                    StateSwitcher.SwitchState<FallingState>();
                    return;
                }
                
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

            _playerConfig.BackSteppingStateConfig.Timer = 0f;
        }
        #endregion
        
        #region OnAmimationEvent Methods
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            Debug.Log("OnAnimationTransitionEvent");
        }
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
        #endregion
        
        //  ON TESTING
        // protected override void OnMovementCanceled(InputAction.CallbackContext context)
        // {
        //     base.OnMovementCanceled(context);
        // }
    }
}