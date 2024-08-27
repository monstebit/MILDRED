using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class DodgeStateConfig
    {
        [field: SerializeField] [field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 2f;
        
        [SerializeField] public Vector3 _lastDodgeDirection;
        [SerializeField] public Vector3  _startDodgePosition ;
        [SerializeField] public float _dodgingTimer; 
        [SerializeField] public float _dodgeDuration;
        [SerializeField] public float _dodgeSpeed;
        [SerializeField] public float _startTime;
        [SerializeField] public float _dodgeDistance ;
        [SerializeField] public float dashTimeRemaining;
    }
}