using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI.Screens
{
	public class EndGameMenu : UIScreen
	{
        [SerializeField] private Transform _newBestScore;
        [SerializeField] private ResizingLabel _scoreLabel;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _shopButton;

        protected override void Awake()
        {
            base.Awake();

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

            if (_shopButton != null)
                _shopButton.onClick.AddListener(OnShopButtonClick);

            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            _scoreLabel.SetValue(_playerService.GetCurrentScore());
            _newBestScore.gameObject.SetActive(_playerService.HasNewBestScore());
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }

        private void OnShopButtonClick()
        {
            _uiService.ShowScreen<ShopMenu>();
        }
    } 
}
