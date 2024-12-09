using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI.Screens
{
    public class HUD : UIScreen
    {
        [SerializeField] private ResizingLabel _scoreLabel;
        [SerializeField] private Button _pauseButton;

        protected override void Awake()
        {
            base.Awake();

            if (_pauseButton != null ) 
                _pauseButton.onClick.AddListener(OnPauseButtonClick);

            _playerService.OnScoreChanged.Add(OnScoreChanged);
            _uiService.RegisterScreen(this);
        }

        private void OnPauseButtonClick()
        {
            _gameService.PauseGame();
        }

        private void OnScoreChanged(int value)
        {
            _scoreLabel.SetValue(value);
        }
    }
}
