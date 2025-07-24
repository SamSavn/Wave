using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI.Screens
{
	public class EndGameMenu : UIScreen
	{
        [SerializeField] private Transform _newBestScore;
        [SerializeField] private ResizingLabel _scoreLabel;
        [SerializeField] private ResizingLabel _bestScoreLabel;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _shopButton;

        protected override void Awake()
        {
            base.Awake();
            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            _scoreLabel.SetValue(_playerService.GetCurrentScore());
            _bestScoreLabel.SetValue(_playerService.GetBestScore());
            _newBestScore.gameObject.SetActive(_playerService.HasNewBestScore());
        }

        protected override void RegisterButtons()
        {
            _uiService.RegisterButton(_homeButton, OnHomeButtonClick);
            _uiService.RegisterButton(_shopButton, OnShopButtonClick);
        }

        protected override void UnregisterButtons()
        {
            _uiService.UnregisterButton(_homeButton);
            _uiService.UnregisterButton(_shopButton);
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }

        private void OnShopButtonClick()
        {
            _gameService.ResetGame();
            _uiService.ShowScreen<ShopMenu>();
        }
    } 
}
