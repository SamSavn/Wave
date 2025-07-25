using UnityEngine;
using UnityEngine.U2D;
using Wave.Services;

namespace Wave.UI.Screens
{
    public abstract class UIScreen : MonoBehaviour
    {
        protected GameService _gameService;
        protected UiService _uiService;
        protected PlayerService _playerService;

        protected virtual void Awake()
        {
            _gameService = ServiceLocator.Instance.Get<GameService>();
            _uiService = ServiceLocator.Instance.Get<UiService>();
            _playerService = ServiceLocator.Instance.Get<PlayerService>();
        }

        public void Enter()
        {
            gameObject.SetActive(true);
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
    } 
}
