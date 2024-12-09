using UnityEngine;
using Wave.Events;

namespace Wave.Services
{
    public class PlayerService : IService
    {
        private const string SHIP_KEY = "Ship";
        private const string COINS_KEY = "Coins";
        private const string BEST_SCORE_KEY = "BestScore";

        private int _shipIndex;
        private int _coins;
        private int _currentScore;
        private bool _newBestScore;

        public EventDisparcher<int> OnScoreChanged { get; } = new EventDisparcher<int>();
        public EventDisparcher<int> OnCoinsChanged { get; } = new EventDisparcher<int>();

        public int GetActiveShipIndex() => _shipIndex;
        public int GetCurrentScore() => _currentScore;
        public int GetBestScore() => PlayerPrefs.GetInt(BEST_SCORE_KEY);
        public int GetCoins() => PlayerPrefs.GetInt(COINS_KEY);
        public bool HasNewBestScore() => _newBestScore;

        public void EquipShip(int shipIndex)
        {
            _shipIndex = shipIndex;
            PlayerPrefs.SetInt(SHIP_KEY, shipIndex);
        }

        public void AddScore(int value)
        {
            _currentScore += value;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public void ResetGameValues()
        {
            _coins = GetCoins();
            _currentScore = 0;
            _newBestScore = false;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public void AddCoins(int value)
        {
            _coins += value;
            OnCoinsChanged?.Invoke(_coins);
        }

        public void SaveScore()
        {
            if (_currentScore <= GetBestScore())
                return;

            PlayerPrefs.SetInt(BEST_SCORE_KEY, _currentScore);
            _newBestScore = true;
        }

        public void SaveCoins()
        {
            PlayerPrefs.SetInt(COINS_KEY, _coins);
        }

        public void SaveEndGameValues()
        {
            SaveScore();
            SaveCoins();
        }
    }
}
