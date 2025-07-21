using Newtonsoft.Json;
using UnityEngine;
using Wave.Data;

namespace Wave.Services
{
    public class DataService : IService
    {
        private const string PLAYER_KEY = "PlayerData";

        public PlayerState LoadPlayerState()
        {
            string json = PlayerPrefs.GetString(PLAYER_KEY);
            Debug.Log($"Loading player data: {json}");

            PlayerData raw = !string.IsNullOrEmpty(json)
                                ? JsonConvert.DeserializeObject<PlayerData>(json)
                                : new PlayerData();

            return new PlayerState(raw);
        }


        public void Save(PlayerState state)
        {
            string jsonData = JsonConvert.SerializeObject(state.GetRawData());
            PlayerPrefs.SetString(PLAYER_KEY, jsonData);
        }
    }
}
