using Wave.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI.Screens
{
    public class MainMenu : UIScreen
    {
        private InputService _inputService;

        [SerializeField] private ResizingLabel _bestScoreLabel;
        [SerializeField] private Button _shopButton;

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

        protected override void RegisterButtons()
        {
            if (_shopButton != null)
                _uiService.RegisterButton(_shopButton, OnShopButtonClick);
        }

        protected override void UnregisterButtons()
        {
            if (_shopButton != null)
                _uiService.UnregisterButton(_shopButton);
        }

        private void OnGameInputDown()
        {
            _gameService.StartGame();
        }

        private void OnShopButtonClick()
        {
            _uiService.ShowScreen<ShopMenu>();
        }
    }
}
