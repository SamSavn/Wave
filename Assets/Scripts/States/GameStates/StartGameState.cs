using Wave.Actors;
using Wave.Environment;

namespace Wave.States.GameStates
{
    public class StartGameState : IState
    {
        private Player _player;
        private Level _level;

        public StartGameState(Player player, Level level)
        {
            _player = player;
            _level = level;
        }

        public void Enter()
        {
            _player.ResetState();
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            _player = null;
            _level = null;
        }
    }
}
