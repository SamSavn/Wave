using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wave.Data;
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
        [SerializeField] private TMP_Text _nameLabel;
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

            if (_versionsContainer != null)
                _versionsContainer.OnVersionSelectionChanged.Add(OnVersionSelectionChanged);

            _uiService.RegisterScreen(this);
        }

        private void OnEnable()
        {
            if (!_uiService.IsScreenActive<ShopMenu>())
                return;

            SetSelection(_playerService.GetEquippedShip().index);
            _sceneService.SetScene(Handlers.SceneType.Shop);            
        }

        private void OnDisable()
        {
            if (!_uiService.IsScreenActive<ShopMenu>())
                return;

            _sceneService.SetScene(Handlers.SceneType.Game);
        }

        protected override void RegisterButtons()
        {
            _uiService.RegisterButton(_leftArrow, OnLeftArrowClick);
            _uiService.RegisterButton(_rightArrow, OnRightArrowClick);
            _uiService.RegisterButton(_buyButton.Button, OnBuyButtonClick);
            _uiService.RegisterButton(_equipButton.Button, OnEquipButtonClick);
            _uiService.RegisterButton(_homeButton, OnHomeButtonClick);
        }

        protected override void UnregisterButtons()
        {
            _uiService.UnregisterButton(_leftArrow);
            _uiService.UnregisterButton(_rightArrow);
            _uiService.UnregisterButton(_buyButton.Button);
            _uiService.UnregisterButton(_equipButton.Button);
            _uiService.UnregisterButton(_homeButton);
        }

        private void SetSelection(int index)
        {
            _currentIndex = index;
            _shipsService.SetSelectedShip(_currentIndex);

            bool equipped = _shipsService.IsShipEquipped(_currentIndex);
            int selectedVersion = equipped
                ? _playerService.GetEquippedShip().version
                : 0;

            _versionsContainer.SetVersions(new ColorVersionsSetData()
            {
                shipInfo = _shipsService.GetInfo(_currentIndex),
                shipsService = _shipsService,
                selectedVersion = selectedVersion,
                shipIndex = _currentIndex
            });

            if (equipped)
            {
                _versionsContainer.EquipVersion(_playerService.GetEquippedShip().version);
            }

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
            _nameLabel.text = _shipsService.GetShipName(_currentIndex).Trim().ToUpper();

            bool unlocked = _shipsService.IsVersionUnlocked(_currentIndex, _currentVersion);
            bool equipped = _shipsService.IsShipEquipped(_currentIndex, _currentVersion);
            _currentPrice = _shipsService.GetPrice(_currentIndex, _currentVersion);

            _leftArrow.gameObject.SetActive(_currentIndex > 0);
            _rightArrow.gameObject.SetActive(_currentIndex < _shipsService.GetShipsCount() - 1);

            _buyButton.gameObject.SetActive(!unlocked);
            _buyButton.Interactable = _playerService.CanBuy(_currentPrice);
            _priceLabel.SetValue(_currentPrice);

            _equipButton.gameObject.SetActive(unlocked && !equipped);
            _equippedLabel.SetActive(equipped);
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
            _versionsContainer.UnlockVersion(_currentVersion);

            Refresh();
        }

        private void OnEquipButtonClick()
        {
            _playerService.EquipShip(_currentIndex, _currentVersion);
            _versionsContainer.EquipVersion(_currentVersion);

            Refresh();
        }

        private void OnHomeButtonClick()
        {
            _gameService.ResetGame();
        }
    } 
}
