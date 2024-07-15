using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Configs
{
    [Serializable]
    public class GroundedStateConfig
    {
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
    }
}