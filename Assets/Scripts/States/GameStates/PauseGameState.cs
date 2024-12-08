using UnityEngine;
using Wave.Actors;
using Wave.Services;
using Wave.UI;

namespace Wave.States.GameStates
{
    public class PauseGameState : IState
    {
        private Player _player;

        public PauseGameState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            Time.timeScale = 0;
            _player.Pause();
            ServiceLocator.Instance.Get<UiService>().ShowScreen<PauseMenu>();
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            _player.Resume();
        }
    }
}
