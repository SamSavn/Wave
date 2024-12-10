using UnityEngine;
using Wave.Events;

namespace Wave.Services
{
    public class PlayerService : IService
    {
        private DataService _dataService;
        private GameService _gameService;

        private int _currentScore;
        private int _currentCoins;
        private bool _newBestScore;

        public EventDisparcher<int> OnScoreChanged { get; } = new EventDisparcher<int>();
        public EventDisparcher<int> OnCoinsChanged { get; } = new EventDisparcher<int>();

        public PlayerService(DataService dataService, GameService gameService)
        {
            _dataService = dataService;
            _gameService = gameService;
        }

        public int GetEquipedShipIndex() => _dataService.GetEquipedShip();
        public int GetCurrentScore() => _currentScore;
        public int GetBestScore() => _dataService.GetBestScore();
        public int GetCoins() => _dataService.GetCoins();
        public bool HasNewBestScore() => _newBestScore;

        public void EquipShip(GameObject ship, int shipIndex)
        {
            _gameService.GetPlayer().SetModel(ship);
            _dataService.SaveEquipedShip(shipIndex);
        }

        public void AddScore(int value)
        {
            _currentScore += value;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public void ResetGameValues()
        {
            _currentCoins = GetCoins();
            _currentScore = 0;
            _newBestScore = false;
            OnScoreChanged?.Invoke(_currentScore);
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

        public void TrySaveBestScore()
        {
            if (_currentScore <= GetBestScore())
                return;

            _dataService.SaveBestScore(_currentScore);
            _newBestScore = true;
        }

        public void SaveCoins() => _dataService.SaveCoins(_currentCoins);

        public void SaveEndGameValues()
        {
            TrySaveBestScore();
            SaveCoins();
        }
    }
}
