using UnityEngine;
using Wave.Events;

namespace Wave.Services
{
    public class PlayerService : IService
    {
        private const string BEST_SCORE_KEY = "BestScore";

        private int _currentScore;
        private bool _newBestScore;

        public EventDisparcher<int> OnScoreChanged { get; } = new EventDisparcher<int>();

        public int GetCurrentScore() => _currentScore;
        public int GetBestScore() => PlayerPrefs.GetInt(BEST_SCORE_KEY);
        public bool HasNewBestScore() => _newBestScore;

        public void AddScore(int value)
        {
            _currentScore += value;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public void ResetScore()
        {
            _currentScore = 0;
            _newBestScore = false;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public void SaveScore()
        {
            if (_currentScore <= GetBestScore())
                return;

            PlayerPrefs.SetInt(BEST_SCORE_KEY, _currentScore);
            _newBestScore = true;
        }
    }
}
