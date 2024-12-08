using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI
{
	public class EndGameMenu : UIScreen
	{
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;

        protected override void Awake()
        {
            base.Awake();

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartButtonClick);

            _uiService.RegisterScreen(this);
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }

        private void OnRestartButtonClick()
        {
            _gameService.RestartLevel();
        }
    } 
}
