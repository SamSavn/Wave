using UnityEngine;
using UnityEngine.UI;

namespace Wave.UI
{
	public class EndGameMenu : UIScreen
	{
        [SerializeField] private Button _homeButton;

        protected override void Awake()
        {
            base.Awake();

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

            _uiService.RegisterScreen(this);
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }
    } 
}
