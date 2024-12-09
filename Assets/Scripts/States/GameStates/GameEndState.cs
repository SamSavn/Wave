using System;
using Wave.Actors;
using Wave.Environment;
using Wave.Services;
using Wave.UI.Screens;

namespace Wave.States.GameStates
{
    public class GameEndState : IState
    {
        private Player _player;
        private Level _level;

        public GameEndState(Player player, Level level)
        {
            _player = player;
            _level = level;
        }

        public void Enter()
        {
            GC.Collect();

            ServiceLocator.Instance.Get<PlayerService>().SaveEndGameValues();
            ServiceLocator.Instance.Get<UiService>().ShowScreen<EndGameMenu>();
            _player.Die();
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
