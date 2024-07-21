using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    /// <summary>
    /// Этот класс используется для обработки событий анимации игрока, таких как начало анимации,
    /// конец и переход между анимациями. Это позволяет синхронизировать логику игрока с анимациями,
    /// обеспечивая более плавный и интерактивный игровой процесс.
    /// </summary>
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        
        private Animator _animator;

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _playerInputHandler.OnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _playerInputHandler.OnMovementStateAnimationExitEvent();
            
            Debug.Log("[ Trigger_Exit_Event ]");
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _playerInputHandler.OnMovementStateAnimationTransitionEvent();
            
            Debug.Log("[ Trigger_Transition_Event ]");
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return _playerInputHandler.PlayerView._animator.IsInTransition(layerIndex);
        }
    }
}