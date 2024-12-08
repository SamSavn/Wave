using Wave.Environment;
using Wave.Services;
using Wave.UI;

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
