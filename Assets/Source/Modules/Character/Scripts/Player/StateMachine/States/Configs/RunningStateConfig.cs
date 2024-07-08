using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Configs
{
    [Serializable]
    public class RunningStateConfig
    {
        [SerializeField, Range(0, 10)] private float _runningSpeed;

        public float RunningSpeed => _runningSpeed;
    }
}