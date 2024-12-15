using Wave.Environment;
using Wave.Services;
using Wave.UI.Screens;

namespace Wave.States.GameStates
{
    public class GamePlayingState : IState
    {
        private Level _level;

        public GamePlayingState(Level level)
        {
            _level = level;
        }

        public void Enter()
        {
            ServiceLocator.Instance.Get<UiService>().ShowScreen<HUD>();
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
