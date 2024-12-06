namespace Wave.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Execute();
    } 
}
