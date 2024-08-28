using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class DodgeStateConfig
    {
        [SerializeField] public AnimationCurve DodgeCurve;
        [SerializeField] public float DodgeTimer;
        [SerializeField] public float Timer;
        [SerializeField] public bool IsDodging;
        [SerializeField] public Vector3 LastDodgeDirection; // Направление последнего шага назад
        
        [Header("_dodgingTimer ?= Timer")]
        [SerializeField] public float _dodgingTimer; 
    }
}