using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private const string IsMovement = "IsMovement";
        private const string IsGrounded = "IsGrounded";
        private const string IsIdling = "IsIdling";
        private const string IsWalking = "IsWalking";
        private const string IsRunning = "IsRunning";
        
        private const string IsAirborne = "IsAirborne";
        private const string IsJumping = "IsJumping";
        private const string IsFalling = "IsFalling";
        
        private const string IsDodging = "IsDodging";
        
        private Animator _animator;
        [SerializeField] public bool DodgeEnds;
        
        public void Initialize() => _animator = GetComponent<Animator>();
        
        public void StartMovement() => _animator.SetBool(IsMovement, true);
        public void StopMovement() => _animator.SetBool(IsMovement, false);
        
        public void StartGrounded() => _animator.SetBool(IsGrounded, true);
        public void StopGrounded() => _animator.SetBool(IsGrounded, false);
        
        public void StartIdling() => _animator.SetBool(IsIdling, true);
        public void StopIdling() => _animator.SetBool(IsIdling, false);
        
        public void StartWalking() => _animator.SetBool(IsWalking, true);
        public void StopWalking() => _animator.SetBool(IsWalking, false);
        
        public void StartRunning() => _animator.SetBool(IsRunning, true);
        public void StopRunning() => _animator.SetBool(IsRunning, false);
        
        // public void StartDodging() => _animator.SetBool(IsDodging, true);
        // public void StopDodging() => _animator.SetBool(IsDodging, false);
        public void StartDodging()
        {
            _animator.SetBool(IsDodging, true);
        }

        public void StopDodging()
        {
            _animator.SetBool(IsDodging, false);
        }
        
        public void StartAirborne() => _animator.SetBool(IsAirborne, true);
        public void StopAirborne() => _animator.SetBool(IsAirborne, false);
        
        public void StartJumping() => _animator.SetBool(IsJumping, true);
        public void StopJumping() => _animator.SetBool(IsJumping, false);

        public void StartFalling() => _animator.SetBool(IsFalling, true);
        public void StopFalling() => _animator.SetBool(IsFalling, false);

        public void DodgeEndTrigger()
        {
            DodgeEnds = true;
        }
        
        public void ResetDodgeEndTrigger()
        {
            DodgeEnds = false;
        }
    }
}