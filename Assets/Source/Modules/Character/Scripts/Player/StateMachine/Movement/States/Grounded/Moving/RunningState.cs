using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class RunningState : MovingState
    {
        private PlayerCompositionRoot _playerCompositionRoot;
        private PlayerConfig _playerConfig;
        private float _startTime;
        
        public RunningState(
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
            Data.MovementSpeedModifier = _playerConfig.RunningStateConfig.SpeedModifier;
            
            base.Enter();

            PlayerView.StartRunning();
            
            _startTime = Time.time;
        }
        
        public override void Update()
        {
            base.Update();

            if (_playerConfig.MovementStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                
                return;
            }
            
            //Этот текст объясняет, что существует определенная логика перехода между состояниями
            //в зависимости от текущих флагов и предыдущих состояний игрока.
            //В данном случае, если игрок был в состоянии ускорения и затем перестал ускоряться,
            //он перейдет в состояние бега, даже если флаг shouldWalk установлен в true.
            //В противном случае, если бы игрок не был в состоянии ускорения, он бы перешел в состояние ходьбы.
            if (_playerConfig.MovementStateConfig.ShouldWalk == false)
            {
                return;
            }

            //Этот код позволяет убедиться, что игрок находится в состоянии ускорения
            //в течение определенного времени, прежде чем перейти в другое состояние
            //(например, бег или ходьбу). Это помогает управлять плавными переходами между
            //состояниями игрока и обеспечивает более естественное поведение персонажа в игре.
            if (Time.time < _startTime + _playerConfig.SprintingStateConfig.RunToWalkTime)
            {
                return;
            }
            
            StopRunning();
        }
        
        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopRunning();
        }
        
        private void StopRunning()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
            }
        }
        
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            
            StateSwitcher.SwitchState<WalkingState>();
        }

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
    }
}