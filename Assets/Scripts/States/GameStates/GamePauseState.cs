using Wave.Actors;
using Wave.Environment;
using Wave.Services;
using Wave.UI.Screens;

namespace Wave.States.GameStates
{
    public class GamePauseState : IState
    {
        private Player _player;
        private Level _level;

        public GamePauseState(Player player, Level level)
        {
            _player = player;
            _level = level;
        }

        public void Enter()
        {
            _player.Pause();
            _level.Pause();

            ServiceLocator.Instance.Get<UiService>().ShowScreen<PauseMenu>();
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            _player.Resume();
            _level.Move();
        }
    }
}
