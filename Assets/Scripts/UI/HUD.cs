using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI
{
    public class HUD : UIScreen
    {
        [SerializeField] private Button _pauseButton;

        protected override void Awake()
        {
            base.Awake();

            if (_pauseButton != null ) 
                _pauseButton.onClick.AddListener(OnPauseButtonClick);

            _uiService.RegisterScreen(this);
        }

        private void OnPauseButtonClick()
        {
            _gameService.PauseGame();
        }
    }
}
