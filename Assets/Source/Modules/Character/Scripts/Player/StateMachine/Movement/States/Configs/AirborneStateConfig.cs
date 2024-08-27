using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class AirborneStateConfig
    {
        [SerializeField, Range(0, 10)] private float _speed = 8f;
        [SerializeField] private JumpingStateConfig _jumpingStateConfig;
        
        public JumpingStateConfig JumpingStateConfig => _jumpingStateConfig;
    }
}