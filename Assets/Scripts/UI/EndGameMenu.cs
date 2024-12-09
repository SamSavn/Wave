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

        protected override void Awake()
        {
            base.Awake();

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

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
    } 
}
