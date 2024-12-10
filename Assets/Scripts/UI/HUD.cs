using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI.Screens
{
    public class HUD : UIScreen
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private Button _pauseButton;

        protected override void Awake()
        {
            base.Awake();

            if (_pauseButton != null ) 
                _pauseButton.onClick.AddListener(OnPauseButtonClick);

            _uiService.RegisterScreen(this);
            _playerService.OnScoreChanged.Add(OnScoreChanged);
        }

        private void OnEnable()
        {
            SetScore(0);
        }

        private void SetScore(int value) => _scoreLabel.text = value.ToString();

        private void OnPauseButtonClick()
        {
            _gameService.PauseGame();
        }

        private void OnScoreChanged(int value)
        {
            if (_uiService.IsScreenActive<HUD>())
                SetScore(value);
        }
    }
}
