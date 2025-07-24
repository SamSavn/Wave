using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI.Screens
{
    public class PauseMenu : UIScreen
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _resumeButton;

        protected override void Awake()
        {
            base.Awake();
            _uiService.RegisterScreen(this);
        }

        protected override void RegisterButtons()
        {
            _uiService.RegisterButton(_homeButton, OnHomeButtonClick);
            _uiService.RegisterButton(_resumeButton, OnResumeButtonClick);
        }

        protected override void UnregisterButtons()
        {
            _uiService.UnregisterButton(_homeButton);
            _uiService.UnregisterButton(_resumeButton);
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }

        private void OnResumeButtonClick()
        {
            _gameService.StartGame();
        }
    }
}
