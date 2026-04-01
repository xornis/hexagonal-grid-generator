#if UNITY_EDITOR

using UnityEditor;

namespace HexDungeon.Editor
{
    [CustomEditor(typeof(HexDebugSettings))]
    public class DebugSettingsEditor : UnityEditor.Editor
    {
        private HexDebugSettings debugSettings;
        
        private void OnEnable()
        {
            debugSettings = (HexDebugSettings)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        public void Draw()
        {
            var debugModeProp = serializedObject.FindProperty("debugMode");
            EditorHelper.DrawProperty(debugModeProp.propertyPath, serializedObject);

            if (debugModeProp.boolValue)
            {
                DrawFields();
                DrawButtons();
            }
        }

        private void DrawFields()
        {
            EditorHelper.DrawProperty("stepDelay", serializedObject);
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Generation", debugSettings.EditorGenerate);
            EditorHelper.DrawButton("Clear Generation", debugSettings.EditorClear);
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
