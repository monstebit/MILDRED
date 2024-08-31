using System.Collections.Generic;
using System.Linq;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    public class PlayerStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;

        public PlayerStateMachine(
            PlayerInputHandler playerInputHandler, 
            CharacterNetworkManager characterNetworkManager,
            PlayerCameraMovement playerCameraMovement)
        {
            StateMachineData data = new StateMachineData();
            
            _states = new List<IState>()
            {
                new IdlingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement,data),
                new WalkingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new RunningState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new SprintingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new DodgingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new BackSteppingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new JumpingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new FallingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
                new LightLandingState(this, playerInputHandler, characterNetworkManager, playerCameraMovement, data),
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<State>() where State : IState
        {
            IState state = _states.FirstOrDefault(state => state is State);
            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        
        public void HandleInput() => _currentState.HandleAllInputs();
        
        public void Update() => _currentState.Update();

        public void LateUpdate() => _currentState.LateUpdate();

        public void OnAnimationEnterEvent()
        {
            _currentState?.OnAnimationEnterEvent();
        }
        
        public void OnAnimationExitEvent()
        {
            _currentState?.OnAnimationExitEvent();
        }
        
        public void OnAnimationTransitionEvent()
        {
            _currentState?.OnAnimationTransitionEvent();
        }
    } 
}