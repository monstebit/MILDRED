namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        void HandleAllInputs();
        void Update();
        void LateUpdate();
        void OnAnimationEnterEvent();
        void OnAnimationExitEvent();
        void OnAnimationTransitionEvent();
    }
}