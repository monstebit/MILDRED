using System;
using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.States.Grounded
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField, Range(0.01f, 1f)] private float _distanceToCheck;
        
        [SerializeField] public bool isTouches { get; private set; }
        
        // private void Update() => isTouches = Physics.CheckSphere(transform.position, _distanceToCheck, _ground);
        private void Update()
        {
            isTouches = Physics.CheckSphere(transform.position, _distanceToCheck, _ground);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, _distanceToCheck);
        }
    }
}