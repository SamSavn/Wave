using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wave.Data;

namespace Wave.CustomEditors
{
	public static class CustomToolsMenu
	{
        private static PlayerData GetData()
        {
            string jsonData = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }

        private static void Save(PlayerData data)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            PlayerPrefs.SetString("PlayerData", jsonData);

            Debug.Log($"<color=green>Data Saved</color>:\n{jsonData}");
        }

        [MenuItem("Wave/Clear PlayerPrefs")]
        public static void ClearAll()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=green>All cleared!</color>");
        }

        [MenuItem("Wave/Player Data/Reset Coins")]
        public static void ResetCoins()
        {
            PlayerData data = GetData();
            data.coins = 0;
            Save(data);
        }

        [MenuItem("Wave/Player Data/Add 100 Coins")]
        public static void AddCoins()
        {
            PlayerData data = GetData();
            data.coins += 100;
            Save(data);
        }

        [MenuItem("Wave/Player Data/Reset Best Score")]
        public static void ResetBestScore()
        {
            PlayerData data = GetData();
            data.bestScore = 0;
            Save(data);
        }

        [MenuItem("Wave/Player Data/Unlock All Ships")]
        public static void UnlockAllShips()
        {
            PlayerData data = GetData();
            data.unlockedShips = new HashSet<ShipData>();

            for (int i = 0; i < 13; i++)
                data.unlockedShips.Add(new ShipData() 
                { 
                    index = i, 
                    version = 0,
                    unlockedVersions = new HashSet<int> { 0, 1, 2, 3, 4 }
                });

            Save(data);
        }

        [MenuItem("Wave/Player Data/Reset Unlocked Ships")]
        public static void ResetUnlockedShips()
        {
            PlayerData data = GetData();
            data.unlockedShips = new HashSet<ShipData> 
            {
                new ShipData()
                {
                    index = 0,
                    version = 0,
                    unlockedVersions = new HashSet<int> { 0 }
                }
            };

            Save(data);
        }
    } 
}
