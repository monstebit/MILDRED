using UnityEngine;

namespace Source.Modules.Character.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private const string IsIdling = "IsIdling";
        private const string IsWalking = "IsWalking";
        private const string IsSprinting = "IsSprinting";
        
        private Animator _animator;

        public void Initialize() => _animator = GetComponent<Animator>();
        public void StartIdling() => _animator.SetBool(IsIdling, true);
        public void StopIdling() => _animator.SetBool(IsIdling, false);
        public void StartWalking() => _animator.SetBool(IsWalking, true);
        public void StopWalking() => _animator.SetBool(IsWalking, false);
        public void StartSprinting() => _animator.SetBool(IsSprinting, true);
        public void StopSprinting() => _animator.SetBool(IsSprinting, false);
    }
}