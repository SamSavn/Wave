using UnityEngine;
using Wave.Data;

namespace Wave.Services
{
    public class DataService : IService
    {
        private const string PLAYER_KEY = "PlayerData";

        private PlayerData _playerData;

        public DataService()
        {
            string jsonData = PlayerPrefs.GetString(PLAYER_KEY);
            _playerData = !string.IsNullOrEmpty(jsonData)
                                ? JsonUtility.FromJson<PlayerData>(jsonData)
                                : new PlayerData();
        }

        public int GetEquipedShip() => _playerData.shipIndex;
        public int GetBestScore() => _playerData.bestScore;
        public int GetCoins() => _playerData.coins;
        public int[] GetUnlockedShips() => _playerData.unlockedShips;

        public void SaveBestScore(int value)
        {
            _playerData.bestScore = value;
            Save();
        }

        public void SaveCoins(int value)
        {
            _playerData.coins = value;
            Save();
        }

        public void SaveUnlockedShips(int[] values)
        {
            _playerData.unlockedShips = values;
            Save();
        }

        public void SaveEquipedShip(int index)
        {
            _playerData.shipIndex = index;
            Save();
        }

        public void Save()
        {
            string jsonData = JsonUtility.ToJson(_playerData);
            PlayerPrefs.SetString(PLAYER_KEY, jsonData);
        }
    }
}
