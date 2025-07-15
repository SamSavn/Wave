using UnityEngine;
using Wave.Events;
using Wave.Settings;

namespace Wave.Services
{
    public class PlayerService : IService
    {
        private DataService _dataService;
        private GameService _gameService;
        private ShipsService _shipService;

        private int _currentCoins;
        private int _currentScore;
        private bool _newBestScore;

        public PlayerState PlayerState { get; private set; }
        public EventDisparcher<int> OnScoreChanged { get; } = new EventDisparcher<int>();
        public EventDisparcher<int> OnCoinsChanged { get; } = new EventDisparcher<int>();

        public PlayerService(DataService dataService, GameService gameService)
        {
            _dataService = dataService;
            _gameService = gameService;

            PlayerState = _dataService.LoadPlayerState();

            if (!PlayerState.IsShipUnlocked(0))
                PlayerState.UnlockShip(0);

            if (PlayerState.EquippedShip == null)
            {
                PlayerState.EquipShip(0, 0);
                _dataService.Save(PlayerState);
            }
        }

        public int GetEquippedShipIndex() => PlayerState.EquippedShip.index;
        public int GetEquippedShipVersion() => PlayerState.EquippedShip.version;
        public int GetCurrentScore() => _currentScore;
        public int GetBestScore() => PlayerState.BestScore;
        public int GetCoins() => PlayerState.Coins;
        public bool HasNewBestScore() => _newBestScore;

        public bool IsShipUnlocked(int index) => PlayerState.IsShipUnlocked(index);
        public bool IsVersionUnlocked(int index, int version) => PlayerState.IsVersionUnlocked(index, version);
        public bool IsShipEquipped(int index, int version) => IsShipEquipped(index) && PlayerState.EquippedShip.version == version;
        public bool IsShipEquipped(int index) => PlayerState.EquippedShip.index == index;
        public void UnlockShip(int index, int version = 0) => PlayerState.UnlockShip(index, version);

        public void EquipShip(int shipIndex, int version)
        {
            if (PlayerState.IsShipEquipped(shipIndex, version))
                return;

            PlayerState.EquipShip(shipIndex, version);
            _dataService.Save(PlayerState);

            _shipService ??= ServiceLocator.Instance.Get<ShipsService>();
            _gameService.GetPlayer().SetModel(_shipService.GetModel(shipIndex, version));
        }

        public void AddScore(int value)
        {
            _currentScore += value;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public void ResetGameValues()
        {
            _currentCoins = PlayerState.Coins;
            _currentScore = 0;
            _newBestScore = false;

            OnScoreChanged?.Invoke(_currentScore);
            OnCoinsChanged?.Invoke(_currentCoins);
        }

        public bool CanBuy(int price) => _currentCoins >= price;

        public void AddCoins(int value, bool save = false)
        {
            if (value < 0 && !CanBuy(Mathf.Abs(value)))
                return;

            _currentCoins += value;
            OnCoinsChanged?.Invoke(_currentCoins);

            if (save)
                SaveCoins();
        }

        public void SaveCoins()
        {
            PlayerState.SetCoins(_currentCoins);
            _dataService.Save(PlayerState);
        }

        public void TrySaveBestScore()
        {
            if (_currentScore <= PlayerState.BestScore)
                return;

            PlayerState.SetBestScore(_currentScore);
            _newBestScore = true;
            _dataService.Save(PlayerState);
        }

        public void SaveEndGameValues()
        {
            TrySaveBestScore();
            SaveCoins();
        }
    }
}