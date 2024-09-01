using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    /// <summary>
    /// Этот класс используется для обработки событий анимации игрока, таких как начало анимации,
    /// конец и переход между анимациями. Это позволяет синхронизировать логику игрока с анимациями,
    /// обеспечивая более плавный и интерактивный игровой процесс.
    /// </summary>
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("_playerInputHandler")] [SerializeField] private PlayerCompositionRoot playerCompositionRoot;
        
        private Animator _animator;

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (InAnimationTransition())
            {
                return;
            }

            playerCompositionRoot.OnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (InAnimationTransition())
            {
                return;
            }

            playerCompositionRoot.OnMovementStateAnimationExitEvent();
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (InAnimationTransition())
            {
                return;
            }

            playerCompositionRoot.OnMovementStateAnimationTransitionEvent();
        }

        private bool InAnimationTransition(int layerIndex = 0)
        {
            return playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}