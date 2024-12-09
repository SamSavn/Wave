using UnityEditor;
using UnityEngine;

namespace Wave.CustomEditors
{
	public static class CustomToolsMenu
	{
        [MenuItem("Wave/Clear Best Score")]
        public static void ClearBestScore()
        {
            DeletePlayerPref("BestScore");
        }

        [MenuItem("Wave/Clear Coins")]
        public static void ClearCoins()
        {
            DeletePlayerPref("Coins");
        }

        [MenuItem("Wave/Clear All")]
        public static void ClearAll()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=green>All cleared!</color>");
        }

        private static void DeletePlayerPref(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"Unable to clear best score: PlayerPrefs does not contain key '{key}'");
                return;
            }

            PlayerPrefs.DeleteKey(key);
            Debug.Log($"<color=green>{key} cleared!</color>");
        }
    } 
}
