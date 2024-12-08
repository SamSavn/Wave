using Wave.Actors;
using Wave.Environment;
using Wave.Services;
using Wave.UI;

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
            ServiceLocator.Instance.Get<UiService>().ShowScreen<MainMenu>();

            _player.ResetState();
            _level.ResetLevel();
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
