namespace Wave.States.PlayerStates
{
    public interface IPlayerState
    {
        void Enter();
        void Exit();
        void Execute();
    } 
}
