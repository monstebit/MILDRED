using UnityEngine;

namespace Source.Modules.Character.Scripts.Player.StateMachine
{
    /// <summary>
    /// Чтобы метод TriggerOnMovementStateAnimation...() работал,
    /// его нужно вызывать в правильный момент, например, используя
    /// Анимационные события или Проверка в методе Update()
    /// </summary>
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerCompositionRoot playerCompositionRoot;
        
        private Animator _animator;

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (InAnimationTransition())
            {
                return;
            }

            playerCompositionRoot.OnMovementStateAnimationEnterEvent();
        }

        /// <summary>
        /// Этот метод вызывает InAnimationTransition()
        /// и проверяет, находится ли анимация в переходе.
        /// Если она находится в переходе (то есть метод InAnimationTransition() возвращает true),
        /// выполнение метода прекращается и ничего не происходит.
        /// Если перехода нет (InAnimationTransition() возвращает false),
        /// значит, анимация находится в состоянии покоя (она закончилась или находится в процессе выполнения),
        /// и в этом случае вызывается OnMovementStateAnimationExitEvent().
        /// </summary>
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

        /// <summary>
        /// Этот метод проверяет, находится ли анимация в состоянии перехода
        /// (например, из одной анимации в другую) на указанном слое аниматора.
        /// Если анимация находится в процессе перехода, этот метод вернет true.
        ///
        /// Если анимация находится в процессе перехода (например, из состояния "Бег" в состояние "Прыжок"),
        /// метод InAnimationTransition() вернет true, и TriggerOnMovementStateAnimationExitEvent() ничего не сделает.
        /// Это значит, что вы не хотите вызывать OnMovementStateAnimationExitEvent() до тех пор, пока переход не завершится.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        private bool InAnimationTransition(int layerIndex = 0)
        {
            return playerCompositionRoot.PlayerView.Animator.IsInTransition(layerIndex);
        }
    }
}