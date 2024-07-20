using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving
{
    public class RunningState : MovingState
    {
        private float _startTime;
        
        private RunningStateConfig _runningStateConfig;
        private SprintingStateConfig _sprintingStateConfig;
        
        private PlayerConfig _playerConfig;
        
        public RunningState(
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
            _runningStateConfig = playerInputHandler.PlayerConfig.RunningStateConfig;
            _sprintingStateConfig = playerInputHandler.PlayerConfig.SprintingStateConfig;
            _playerConfig = playerInputHandler.PlayerConfig;
        }

        #region IState METHODS
        public override void Enter()
        {
            base.Enter();

            Data.MovementSpeedModifier = _runningStateConfig.SpeedModifier;

            _startTime = Time.time;
            
            PlayerView.StartRunning();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerView.StopRunning();
        }

        public override void Update()
        {
            base.Update();
            
            //  SPRINTING
            if (_playerConfig.SprintingStateConfig.ShouldSprint)
            {
                StateSwitcher.SwitchState<SprintingState>();
                return;
            }

            //Этот текст объясняет, что существует определенная логика перехода между состояниями
            //в зависимости от текущих флагов и предыдущих состояний игрока.
            //В данном случае, если игрок был в состоянии ускорения и затем перестал ускоряться,
            //он перейдет в состояние бега, даже если флаг shouldWalk установлен в true.
            //В противном случае, если бы игрок не был в состоянии ускорения, он бы перешел в состояние ходьбы.
            if (!_playerConfig.MovementStateConfig.ShouldWalk)
            {
                return;
            }

            //Этот код позволяет убедиться, что игрок находится в состоянии ускорения
            //в течение определенного времени, прежде чем перейти в другое состояние
            //(например, бег или ходьбу). Это помогает управлять плавными переходами между
            //состояниями игрока и обеспечивает более естественное поведение персонажа в игре.
            if (Time.time < _startTime + _sprintingStateConfig.RunToWalkTime)
            {
                return;
            }

            StopRunning();
        }
        #endregion

        #region MAIN METHODS
        private void StopRunning()
        {
            if (Data.MovementInput == Vector2.zero)
            {
                StateSwitcher.SwitchState<IdlingState>();
                
                return;
            }
            
            //
            // if (_sprintingStateConfig.ShouldSprint)
            //     StateSwitcher.SwitchState<SprintingState>();
            
            StateSwitcher.SwitchState<WalkingState>();
        }
        #endregion
        
        #region INPUT METHODS
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            
            StateSwitcher.SwitchState<WalkingState>();
        }
        
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }
        
        protected override void OnSprintPerformed(InputAction.CallbackContext context)
        {
            base.OnSprintPerformed(context);
        }
        
        protected override void OnSprintCanceled(InputAction.CallbackContext context)
        {
            base.OnSprintCanceled(context);
        }
        #endregion
    }
}