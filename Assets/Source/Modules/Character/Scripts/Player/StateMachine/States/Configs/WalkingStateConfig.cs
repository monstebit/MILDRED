using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Configs
{
    [Serializable]
    public class WalkingStateConfig
    {
        [SerializeField, Range(0, 10)] private float _walkingSpeed;

        public float WalkingSpeed => _walkingSpeed;
    }
}