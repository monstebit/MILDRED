using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Configs
{
    [Serializable]
    public class SprintingStateConfig
    {
        [SerializeField, Range(0, 10)] private float _sprintingSpeed;

        public float SprintingSpeed => _sprintingSpeed;
    }
}