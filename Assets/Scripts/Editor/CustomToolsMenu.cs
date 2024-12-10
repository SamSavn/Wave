using UnityEditor;
using UnityEngine;

namespace Wave.CustomEditors
{
	public static class CustomToolsMenu
	{
        [MenuItem("Wave/Clear PlayerPrefs")]
        public static void ClearAll()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=green>All cleared!</color>");
        }
    } 
}
