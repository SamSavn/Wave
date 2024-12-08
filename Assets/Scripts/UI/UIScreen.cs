using UnityEngine;
using Wave.Services;

namespace Wave.UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        protected GameService _gameService;
        protected UiService _uiService;

        protected virtual void Awake()
        {
            _gameService = ServiceLocator.Instance.Get<GameService>();
            _uiService = ServiceLocator.Instance.Get<UiService>();            
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
