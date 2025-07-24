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
            _uiService.RegisterScreen(this);
            _playerService.OnScoreChanged.Add(OnScoreChanged);
        }

        private void OnEnable()
        {
            SetScore(0);
        }

        protected override void RegisterButtons()
        {
            _uiService.RegisterButton(_pauseButton, OnPauseButtonClick);
        }

        protected override void UnregisterButtons()
        {
            _uiService.UnregisterButton(_pauseButton);
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
