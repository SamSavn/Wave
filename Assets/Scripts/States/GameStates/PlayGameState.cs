using Wave.Environment;

namespace Wave.States.GameStates
{
    public class PlayGameState : IState
    {
        private Level _level;

        public PlayGameState(Level level)
        {
            _level = level;
        }

        public void Enter()
        {
            _level.StartMoving();
        }

        public void Execute()
        {
        }

        public void Exit()
        {
            _level = null;
        }
    }
}
