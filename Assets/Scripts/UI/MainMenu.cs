using Wave.Services;

namespace Wave.UI
{
    public class MainMenu : UIScreen
    {
        private InputService _inputService;

        protected override void Awake()
        {
            base.Awake();
            _inputService = ServiceLocator.Instance.Get<InputService>();
            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            _inputService.OnGameInputDown.Add(OnGameInputDown);
        }

        private void OnDisable()
        {
            _inputService.OnGameInputDown.Remove(OnGameInputDown);
        }

        private void OnGameInputDown()
        {
            _gameService.StartGame();
        }
    }
}
