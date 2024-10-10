using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction
{
    public class DodgingState : StaticActionState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        
        public DodgingState(
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
         
        public override void Enter()
        {
            base.Enter();
            
            if (InAnimationTransition())
            {
                return;
            }
            
            // PlayerView.StartDodging();
            PlayerView.UpdateState("IsDodging", true);
            
            _playerConfig.DodgeStateConfig.Timer = 0f;
            Keyframe LastFrame = _playerConfig.DodgeStateConfig.DodgeCurve[_playerConfig.DodgeStateConfig.DodgeCurve.length - 1];
            _playerConfig.DodgeStateConfig.DodgeTimer = LastFrame.time;
        }
        
        public override void Exit()
        {
            base.Exit();
            
            // PlayerView.StopDodging();
            PlayerView.UpdateState("IsDodging", false);
        }
        
        public override void Update()
        {
            base.Update();

            HandleDodge();
        }

        private void HandleDodge()
        {
            _playerConfig.DodgeStateConfig.Timer += Time.deltaTime;

            if (_playerConfig.DodgeStateConfig.Timer < _playerConfig.DodgeStateConfig.DodgeTimer)
            {
                float speed = _playerConfig.DodgeStateConfig.DodgeCurve.Evaluate(_playerConfig.DodgeStateConfig.Timer);
                _playerCompositionRoot.CharacterController.Move(PlayerView.transform.forward * speed * Time.deltaTime);
                return;
            }
            
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
        
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return _playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}