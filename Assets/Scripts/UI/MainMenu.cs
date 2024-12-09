using Wave.Services;
using UnityEngine;

namespace Wave.UI.Screens
{
    public class MainMenu : UIScreen
    {
        private InputService _inputService;

        [SerializeField] private ResizingLabel _bestScoreLabel;

        protected override void Awake()
        {
            base.Awake();
            _inputService = ServiceLocator.Instance.Get<InputService>();
            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            _inputService.OnGameInputDown.Add(OnGameInputDown);
            _bestScoreLabel.SetValue(_playerService.GetBestScore());
        }

        private void OnDisable()
        {
            _inputService.OnGameInputDown.Remove(OnGameInputDown);
        }

        private void OnGameInputDown()
        {
            _gameService.StartGame();
        }
    }
}
