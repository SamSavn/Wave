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
        [SerializeField] private ShipsVersionsContainer _versionsContainer;

        [Space]
        [SerializeField] private TextButton _buyButton;
        [SerializeField] private TextButton _equipButton;
        [SerializeField] private Button _homeButton;

        [Space]
        [SerializeField] private ResizingLabel _priceLabel;
        [SerializeField] private GameObject _equippedLabel;

        private SceneService _sceneService;
        private ShipsService _shipsService;

        private int _currentIndex = 0;
        private int _currentVersion = 0;
        private int _currentPrice = 0;

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
                _buyButton.OnClick.AddListener(OnBuyButtonClick);

            if (_equipButton != null)
                _equipButton.OnClick.AddListener(OnEquipButtonClick);

            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeButtonClick);

            if (_versionsContainer != null)
                _versionsContainer.OnVersionSelectionChanged.Add(OnVersionSelectionChanged);

            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            if (!_uiService.IsScreenActive<ShopMenu>())
                return;

            SetSelection(_playerService.GetEquippedShipIndex());
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
            _shipsService.SetSelectedShip(_currentIndex);

            int selectedVersion = _shipsService.IsShipEquipped(_currentIndex)
                ? _playerService.GetEquippedShipVersion()
                : 0;

            _versionsContainer.SetVersions(_shipsService.GetStats(_currentIndex), selectedVersion);
            SetVersion(selectedVersion);
            Refresh();
        }

        private void SetVersion(int index)
        {
            _currentVersion = index;
            _shipsService.SetShipVersion(_currentIndex, _currentVersion);
            Refresh();
        }

        private void Refresh()
        {
            bool unlocked = _shipsService.IsVersionUnlocked(_currentIndex, _currentVersion);
            bool equipped = _shipsService.IsShipEquipped(_currentIndex, _currentVersion);
            _currentPrice = _shipsService.GetPrice(_currentIndex, _currentVersion);

            _leftArrow.gameObject.SetActive(_currentIndex > 0);
            _rightArrow.gameObject.SetActive(_currentIndex < _shipsService.GetShipsCount() - 1);

            _buyButton.gameObject.SetActive(!unlocked);
            _buyButton.Interactable = _playerService.CanBuy(_currentPrice);

            _equipButton.gameObject.SetActive(unlocked && !equipped);
            _equippedLabel.SetActive(equipped);

            _priceLabel.SetValue(_currentPrice);
            _priceLabel.gameObject.SetActive(!unlocked);
        }

        private void OnLeftArrowClick()
        {
            SetSelection(_currentIndex - 1);
        }

        private void OnRightArrowClick()
        {
            SetSelection(_currentIndex + 1);
        }

        private void OnVersionSelectionChanged(int index)
        {
            SetVersion(index);
        }

        private void OnBuyButtonClick()
        {
            _playerService.AddCoins(-_currentPrice, save: true);
            _shipsService.UnlockShip(_currentIndex, _currentVersion);
            Refresh();
        }

        private void OnEquipButtonClick()
        {
            _playerService.EquipShip(_currentIndex, _currentVersion);
            Refresh();
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }
    } 
}
