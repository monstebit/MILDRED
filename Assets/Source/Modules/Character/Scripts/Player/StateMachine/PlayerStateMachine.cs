using System.Collections.Generic;
using System.Linq;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Airborn;
using Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    public class PlayerStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;

        public PlayerStateMachine(
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager,
            CameraMovement cameraMovement)
        {
            StateMachineData data = new StateMachineData();
            
            _states = new List<IState>()
            {
                new IdlingState(this, playerInputHandler, characterNetworkManager, cameraMovement,data),
                new WalkingState(this, playerInputHandler, characterNetworkManager, cameraMovement, data),
                new SprintingState(this, playerInputHandler, characterNetworkManager, cameraMovement, data),
                new JumpingState(this, playerInputHandler, characterNetworkManager, cameraMovement, data),
                new FallingState(this, playerInputHandler, characterNetworkManager, cameraMovement, data),
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<State>() where State : IState
        {
            IState state = _states.FirstOrDefault(state => state is State); //  НАШЛИ ТЕКУЩЕЕ СОСТОЯНИЕ
            _currentState.Exit();   //  ВЫХОДИМ ИЗ ТЕКУЩЕГО СОСТОЯНИЯ
            _currentState = state;  //  ПРИСВАЕВАЕМ ТЕКУЩЕМУ СОСТОЯНИЮ НОВОЕ
            _currentState.Enter();  //  ВХОДИМ В НОВОЕ СОСТОЯНИЕ
        }
        
        public void HandleInput() => _currentState.HandleInput();
        
        public void Update() => _currentState.Update();

        public void LateUpdate() => _currentState.LateUpdate();
    } 
}