using UnityEngine;
using Wave.Actors;
using Wave.Environment;
using Wave.Handlers;
using Wave.States;
using Wave.States.GameStates;
using Wave.UI.Screens;

namespace Wave.Services
{
	public class GameService : IService
	{
        private readonly StateMachine _stateMachine;
		private readonly UiService _uiService;
		private readonly SceneService _sceneService;
		private PlayerService _playerService;
		private ShipsService _shipsService;

        private Player _player;
		private Level _level;

        public GameService(UpdateService updateService, UiService uiService, SceneService sceneService)
		{
			Application.targetFrameRate = 30;
			_stateMachine = new StateMachine();
			_uiService = uiService;
			_sceneService = sceneService;
        }

		public void ResetGame() 
		{
			if (!_stateMachine.IsInState<GameStartState>())
			{
                _stateMachine.SetState(new GameStartState(_player, _level)); 
				return;
            }

			_uiService.ShowScreen<MainMenu>();
			_sceneService.SetScene(SceneType.Game);
        }

		public void StartGame() => _stateMachine.SetState(new GamePlayingState(_level));
		public void PauseGame() => _stateMachine.SetState(new GamePauseState(_player, _level));
		public void EndGame() => _stateMachine.SetState(new GameEndState(_player, _level));

        public Player GetPlayer() => _player;

		public void SetPlayer(Player player)
		{
			_playerService ??= ServiceLocator.Instance.Get<PlayerService>();
			_shipsService ??= ServiceLocator.Instance.Get<ShipsService>();

            _player = player;

            (int index, int version) ship = _playerService.GetEquippedShip();
			GameObject model = _shipsService.GetModel(ship.index, ship.version);
			Vector3 trailOrigin = _shipsService.GetTrailOrigin(ship.index);

            _player.SetModel(model, trailOrigin);
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
