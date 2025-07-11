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

        private int _currentIndex = -1;

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

            SetSelection(_playerService.GetEquipedShipIndex());
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
            _versionsContainer.SetVersions(_shipsService.GetStats(_currentIndex));

            SetVersion(0);
            Refresh();
        }

        private void SetVersion(int index)
        {
            _shipsService.SetShipVersion(_currentIndex, index);
        }

        private void Refresh()
        {
            bool unlocked = _shipsService.IsShipUnlocked(_currentIndex);
            bool equipped = _shipsService.IsShipEquiped(_currentIndex);

            _leftArrow.gameObject.SetActive(_currentIndex > 0);
            _rightArrow.gameObject.SetActive(_currentIndex < _shipsService.GetShipsCount() - 1);

            _buyButton.gameObject.SetActive(!unlocked);
            _buyButton.Interactable = _playerService.CanBuy(_shipsService.GetShipPrice(_currentIndex));

            _equipButton.gameObject.SetActive(unlocked && !equipped);
            _equippedLabel.SetActive(equipped);

            _priceLabel.SetValue(_shipsService.GetShipPrice(_currentIndex));
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
            _playerService.AddCoins(-_shipsService.GetShipPrice(_currentIndex), save: true);
            _shipsService.UnlockShip(_currentIndex);
            Refresh();
        }

        private void OnEquipButtonClick()
        {
            _playerService.EquipShip(_shipsService.GetShip(_currentIndex), _currentIndex);
            Refresh();
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }
    } 
}
