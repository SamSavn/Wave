using System.IO;
using UnityEditor;
using UnityEngine;
using Wave.Settings;

namespace Wave.CustomEditors
{
	public class ShipStatsGenerator : EditorWindow
    {
        private string _prefabsFolderPath;
        private string _targetFolderPath;
        private bool _foldersSelected;

        [MenuItem("Wave/Ship Stats Generator")]
        public static void ShowWindow()
        {
            GetWindow<ShipStatsGenerator>("Ship Stats Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Ship Stats Generator", EditorStyles.boldLabel);

            if (GUILayout.Button("Select Prefabs Folder"))
            {
                string selectedFolder = EditorUtility.OpenFolderPanel("Select Prefabs Folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(selectedFolder) && selectedFolder.StartsWith(Application.dataPath))
                {
                    _prefabsFolderPath = selectedFolder.Replace(Application.dataPath, "Assets");
                }
                else
                {
                    Debug.LogError("Invalid folder selection. Please select a folder within the Assets directory.");
                }
            }

            if (!string.IsNullOrEmpty(_prefabsFolderPath))
            {
                EditorGUILayout.LabelField("Prefabs Folder: ", _prefabsFolderPath);
            }

            if (GUILayout.Button("Select Target Folder"))
            {
                string selectedFolder = EditorUtility.OpenFolderPanel("Select Target Folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(selectedFolder) && selectedFolder.StartsWith(Application.dataPath))
                {
                    _targetFolderPath = selectedFolder.Replace(Application.dataPath, "Assets");
                    _foldersSelected = true;
                }
                else
                {
                    Debug.LogError("Invalid folder selection. Please select a folder within the Assets directory.");
                }
            }

            if (!string.IsNullOrEmpty(_targetFolderPath))
            {
                EditorGUILayout.LabelField("Target Folder: ", _targetFolderPath);
            }

            if (_foldersSelected && GUILayout.Button("Generate Stats"))
            {
                GenerateStats();
            }
        }

        private void GenerateStats()
        {
            if (string.IsNullOrEmpty(_prefabsFolderPath) || string.IsNullOrEmpty(_targetFolderPath))
            {
                Debug.LogError("Both the prefabs folder and target folder must be selected.");
                return;
            }

            string[] prefabPaths = Directory.GetFiles(_prefabsFolderPath, "*.prefab", SearchOption.AllDirectories);
            if (prefabPaths.Length == 0)
            {
                Debug.LogWarning("No prefabs found in the selected folder.");
                return;
            }

            foreach (string prefabPath in prefabPaths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning($"Could not load prefab at path: {prefabPath}");
                    continue;
                }

                // Create the ScriptableObject instance
                ShipStats shipStats = ScriptableObject.CreateInstance<ShipStats>();
                shipStats.name = prefab.name;
                AssignStats(shipStats, prefab);

                // Save the ScriptableObject to the target folder
                string assetPath = Path.Combine(_targetFolderPath, $"{prefab.name}_ShipStats.asset");
                AssetDatabase.CreateAsset(shipStats, assetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Ship Stats generation completed.");
        }

        private void AssignStats(ShipStats stats, GameObject prefab)
        {
            var statsType = stats.GetType();
            statsType.GetField("_prefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(stats, prefab);
            statsType.GetField("_mass", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(stats, 0.7f);
            statsType.GetField("_power", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(stats, 30f);
            statsType.GetField("_speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(stats, 70f);
        }
    }
}
