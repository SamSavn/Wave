using Wave.Actors;
using Wave.Environment;
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
			_stateMachine = new StateMachine();
			ServiceLocator.Instance.Get<InputService>().OnGameInputDown.Add(OnGameInputDown);
        }

		public void ResetGame() => _stateMachine.SetState(new StartGameState(_player, _level));
		public void StartGame() => _stateMachine.SetState(new PlayGameState(_level));
		public void EndGame() => _stateMachine.SetState(new EndGameState(_player, _level));

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
            if (_player != null && _level != null)
                ResetGame();
        }

		//todo: remove once UI is on
		private void OnGameInputDown()
		{
			if (_stateMachine.IsInState<StartGameState>()) StartGame();
			else if (_stateMachine.IsInState<EndGameState>()) ResetGame();
		}
	} 
}
