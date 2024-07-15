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
        public float Speed => _speed;
        public float BaseGravity => 2f * _jumpingStateConfig.MaxHeight /
                                    (_jumpingStateConfig.TimeToReachMaxHeight * _jumpingStateConfig.TimeToReachMaxHeight);
    }
}