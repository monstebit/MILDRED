namespace Source.Modules.Character.Scripts.Player.StateMachine.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        void HandleAllInputs();
        void Update();
        void LateUpdate();
    }
}