using System.Collections.Generic;
using System.Linq;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.States;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    public class PlayerStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;

        public PlayerStateMachine(PlayerInputHandler playerInputHandler, CharacterNetworkManager characterNetworkManager)
        {
            StateMachineData data = new StateMachineData();
            
            _states = new List<IState>()
            {
                new IdlingState(this, playerInputHandler, characterNetworkManager, data),
                new WalkingState(this, playerInputHandler, characterNetworkManager, data),
                new SprintingState(this, playerInputHandler, characterNetworkManager, data),
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<State>() where State : IState
        {
            //  НАШЛИ ТЕКУЩЕЕ СОСТОЯНИЕ
            IState state = _states.FirstOrDefault(state => state is State);
            //  ВЫХОДИМ ИЗ ТЕКУЩЕГО СОСТОЯНИЯ
            _currentState.Exit();
            //  ПРИСВАЕВАЕМ ТЕКУЩЕМУ СОСТОЯНИЮ НОВОЕ
            _currentState = state;
            //  ВХОДИМ В НОВОЕ СОСТОЯНИЕ
            _currentState.Enter();
        }
        
        public void HandleInput() => _currentState.HandleInput();
        
        public void Update() => _currentState.Update();

        public void LateUpdate() => _currentState.LateUpdate();
    } 
}