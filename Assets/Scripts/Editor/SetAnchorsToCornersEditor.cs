using UnityEditor;
using UnityEngine;

namespace Wave.CustomEditors
{
    [CustomEditor(typeof(SetAnchorsToCorners))]
    public class SetAnchorsToCornersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SetAnchorsToCorners script = (SetAnchorsToCorners)target;

            if (GUILayout.Button("Process"))
            {
                script.Process();
            }

            if (GUILayout.Button("Process Children"))
            {
                script.ProcessChildren();
            }
        }
    } 
}
