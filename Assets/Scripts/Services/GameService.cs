using UnityEngine;
using Wave.Actors;
using Wave.Environment;
using Wave.Handlers;
using Wave.States;
using Wave.States.GameStates;

namespace Wave.Services
{
	public class GameService : IService
	{
        private readonly StateMachine _stateMachine;

		private Player _player;
		private Level _level;

        public GameService(UpdateService updateService)
		{
			Application.targetFrameRate = 30;
			_stateMachine = new StateMachine();
        }

		public void ResetGame() => _stateMachine.SetState(new GameStartState(_player, _level));
		public void StartGame() => _stateMachine.SetState(new GamePlayingState(_level));
		public void PauseGame() => _stateMachine.SetState(new GamePauseState(_player, _level));
		public void EndGame() => _stateMachine.SetState(new GameEndState(_player, _level));

		public Player GetPlayer() => _player;

		public void SetPlayer(Player player)
		{
            _player = player;
			TrySetGame();
        }

		public void SetLevel(Level level)
		{
			_level = level;
			TrySetGame();
		}

		private void TrySetGame()
        {
            if (_player == null || _level == null)
                return;

            ServiceLocator.Instance.Get<SceneService>().SetScene(SceneType.Game);
            ResetGame();
        }
    } 
}
