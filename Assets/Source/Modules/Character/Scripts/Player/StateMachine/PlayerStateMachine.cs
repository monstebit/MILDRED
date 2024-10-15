using System.Collections.Generic;
using System.Linq;
using Source.Modules.Character.Scripts.Player.StateMachine.Interfaces;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Airborne;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Landing;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.Moving;
using Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded.StaticAction;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    public class PlayerStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;
        private PlayerCompositionRoot _playerCompositionRoot;

        public PlayerStateMachine(PlayerCompositionRoot playerCompositionRoot)
        {
            _playerCompositionRoot = playerCompositionRoot;
            
            StateMachineData data = new StateMachineData();
            
            _states = new List<IState>()
            {
                new IdlingState(this, playerCompositionRoot, data),
                new WalkingState(this, playerCompositionRoot, data),
                new RunningState(this, playerCompositionRoot, data),
                new SprintingState(this, playerCompositionRoot, data),
                new DodgingState(this, playerCompositionRoot, data),
                new BackSteppingState(this, playerCompositionRoot, data),
                new JumpingState(this, playerCompositionRoot, data),
                new FallingState(this, playerCompositionRoot, data),
                new LightLandingState(this, playerCompositionRoot, data),
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<State>() where State : IState
        {
            if (IsOwner() == false)
            {
                return;
            }
            
            IState state = _states.FirstOrDefault(state => state is State);
            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        
        public void HandleInput()
        {
            if (IsOwner() == false)
            {
                return;
            }
            
            _currentState.HandleAllInputs();
        }
        
        public void Update()
        {
            // if (_playerCompositionRoot.PlayerNetworkSynchronizer.IsClient &&
            //     _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner)
            // {
            //     _playerCompositionRoot.asUpdateClientPositionAndRotationServerRpc(
            //         _playerCompositionRoot.PlayerView.transform.position, _playerCompositionRoot.PlayerView.transform.rotation);
            // }
            
            if (IsOwner() == false)
            {
                return;
            }
            
            _currentState.Update();
        }

        public void LateUpdate()
        {
            if (IsOwner() == false)
            {
                return;
            }
            
            _currentState.LateUpdate();
        }

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
        
        private bool IsOwner()
        {
            return _playerCompositionRoot.PlayerNetworkSynchronizer.IsOwner;
        }
    } 
}