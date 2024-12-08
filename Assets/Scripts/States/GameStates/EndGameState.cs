using UnityEngine;
using Wave.Actors;
using Wave.Environment;
using Wave.Services;
using Wave.UI;

namespace Wave.States.GameStates
{
    public class EndGameState : IState
    {
        private Player _player;
        private Level _level;

        public EndGameState(Player player, Level level)
        {
            _player = player;
            _level = level;
        }

        public void Enter()
        {
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
