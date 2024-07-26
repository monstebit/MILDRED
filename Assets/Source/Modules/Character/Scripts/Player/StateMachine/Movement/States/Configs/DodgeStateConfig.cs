using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class DodgeStateConfig
    {
        [field: SerializeField] [field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 2f;
        
        public float _dodgeDuration;
        public float _dodgeSpeed;
        public float _startTime;
        public float _dodgeDistance ;
        public Vector3  _startDodgePosition ;
        private float dashTimeRemaining;
    }
}