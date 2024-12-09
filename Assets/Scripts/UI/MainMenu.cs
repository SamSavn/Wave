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

            if (_shopButton != null )
                _shopButton.onClick.AddListener(OnShopButtonClick);

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

        private void OnShopButtonClick()
        {
            _uiService.ShowScreen<ShopMenu>();
        }
    }
}
