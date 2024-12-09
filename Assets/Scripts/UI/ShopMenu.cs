using UnityEngine;
using UnityEngine.UI;
using Wave.Services;

namespace Wave.UI.Screens
{
    public class ShopMenu : UIScreen
    {
        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;

        [Space]
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _homeButton;

        [Space]
        [SerializeField] private ResizingLabel _priceLabel;

        private const int SHIPS_PRICE = 100;

        private SceneService _sceneService;
        private ShipsService _shipsService;

        private int _currentIndex;

        protected override void Awake()
        {
            base.Awake();

            _sceneService = ServiceLocator.Instance.Get<SceneService>();
            _shipsService = ServiceLocator.Instance.Get<ShipsService>();

            if (_leftArrow != null)
                _leftArrow.onClick.AddListener(OnLeftArrowClick);

            if (_rightArrow != null)
                _rightArrow.onClick.AddListener(OnRightArrowClick);

            if (_buyButton != null)
                _buyButton.onClick.AddListener(OnBuyButtonClick);

            if (_equipButton != null)
                _equipButton.onClick.AddListener(OnEquipButtonClick);

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

            if (_priceLabel != null)
                _priceLabel.SetValue(SHIPS_PRICE);

            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            if (!_uiService.IsScreenActive<ShopMenu>())
                return;

            SetSelection(_playerService.GetActiveShipIndex());
            _sceneService.SetScene(Handlers.SceneType.Shop);       
        }

        private void OnDisable()
        {
            if (!_uiService.IsScreenActive<ShopMenu>())
                return;

            _sceneService.SetScene(Handlers.SceneType.Game);       
        }

        private void SetSelection(int index)
        {
            _currentIndex = index;
            _leftArrow.gameObject.SetActive(index > 0);
            _rightArrow.gameObject.SetActive(index < _shipsService.GetShipsCount() - 1);

            _shipsService.SetSelectedShip(_currentIndex);
        }

        private void OnLeftArrowClick()
        {
            SetSelection(_currentIndex - 1);
        }

        private void OnRightArrowClick()
        {
            SetSelection(_currentIndex + 1);
        }

        private void OnBuyButtonClick()
        {
            _playerService.AddCoins(-SHIPS_PRICE);
            _playerService.SaveCoins();
        }

        private void OnEquipButtonClick()
        {
            _playerService.EquipShip(_currentIndex);
        }

        private void OnHomeButtonClick()
        {
            _uiService.ShowScreen<MainMenu>();
        }
    } 
}
