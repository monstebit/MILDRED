using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States.Grounded
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField, Range(0.01f, 1f)] private float _distanceToCheck;
        
        public bool isTouches { get; private set; }
        
        private void Update()
        {
            isTouches = Physics.CheckSphere(transform.position, _distanceToCheck, _ground);
        }

        #region ОТРИСОВКА СФЕРЫ
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, _distanceToCheck);
        }
        #endregion

    }
}