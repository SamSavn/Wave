using Wave.Actors;
using Wave.Environment;
using Wave.Services;
using Wave.UI.Screens;

namespace Wave.States.GameStates
{
    public class GameStartState : IState
    {
        private Player _player;
        private Level _level;

        public GameStartState(Player player, Level level)
        {
            _player = player;
            _level = level;
        }

        public void Enter()
        {
            ServiceLocator.Instance.Get<UiService>().ShowScreen<MainMenu>();
            ServiceLocator.Instance.Get<PlayerService>().ResetGameValues();

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
