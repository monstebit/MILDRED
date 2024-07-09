using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Configs
{
    [Serializable]
    public class JumpingStateConfig
    {
        [SerializeField, Range(0, 10)] private float _maxHeight = 2f;
        [SerializeField, Range(0, 10)] private float _timeToReachMaxHeight = 0.6f;
        
        public float StartYVelocity => 2 * _maxHeight / _timeToReachMaxHeight;
        public float MaxHeight => _maxHeight;
        public float TimeToReachMaxHeight => _timeToReachMaxHeight;
    }
}