namespace Source.Modules.Character.Scripts.Player.StateMachine.Movement.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        void HandleAllInputs();
        void Update();
        void LateUpdate();
        void OnAnimationEnterEvent();   //  НАПРИМЕР, ИГНОРИРОВАТЬ УРОН ПРИ АТАКЕ ПО ИГРОКУ
        void OnAnimationExitEvent();    //  НАПРИМЕР, ЗАКОНЧИТЬ ИГНОРИРОВАНИЕ УРОНА НА ПОСЛЕЛНЕМ КАДРЕ АНИМАЦИИ
        void OnAnimationTransitionEvent();
    }
}