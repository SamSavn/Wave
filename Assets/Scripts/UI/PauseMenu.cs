﻿using UnityEngine;
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

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

            if (_resumeButton != null)
                _resumeButton.onClick.AddListener(OnResumeButtonClick);

            _uiService.RegisterScreen(this);
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
