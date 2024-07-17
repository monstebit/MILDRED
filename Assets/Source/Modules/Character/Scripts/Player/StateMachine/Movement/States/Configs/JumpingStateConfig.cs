using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class JumpingStateConfig
    {
        [SerializeField, Range(0, 10)] private float _maxHeight = 2f;
        [SerializeField, Range(0, 10)] private float _timeToReachMaxHeight = 0.6f;
        [SerializeField, Range(0, 10)] private float _jumpDistance = 5f;    //  TEST
        
        public float StartYVelocity => 2 * _maxHeight / _timeToReachMaxHeight;
        
        public float MaxHeight => _maxHeight;
        public float TimeToReachMaxHeight => _timeToReachMaxHeight;
    }
}