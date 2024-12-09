using UnityEditor;
using UnityEngine;

namespace Wave.CustomEditors
{
	public static class CustomToolsMenu
	{
        [MenuItem("Wave/Clear Best Score")]
        public static void ClearBestScore()
        {
            string key = "BestScore";

            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"Unable to clear best score: PlayerPrefs does not contain key '{key}'");
                return;
            }

            PlayerPrefs.DeleteKey(key);
            Debug.Log("<color=green>Best Score cleared!</color>");
        }
    } 
}
