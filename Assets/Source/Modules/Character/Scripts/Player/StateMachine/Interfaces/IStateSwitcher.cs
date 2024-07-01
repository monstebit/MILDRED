using Source.Modules.Character.Scripts.Player.StateMachine.States;

namespace Source.Modules.Character.Scripts.Player.StateMachine.Interfaces
{
    public interface IStateSwitcher
    {
        void SwitchState<State>() where State : IState;
    }
}