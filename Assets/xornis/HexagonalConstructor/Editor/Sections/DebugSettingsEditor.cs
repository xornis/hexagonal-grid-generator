#if UNITY_EDITOR

using UnityEditor;

namespace HexDungeon.Editor
{
    [CustomEditor(typeof(HexDebugSettings))]
    public class DebugSettingsEditor : UnityEditor.Editor
    {
        private HexDebugSettings debugSettings;
        private bool foldout = true;

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
            EditorHelper.DrawFoldout(ref foldout, debugSettings.GetType().Name, () =>
            {
                var debugModeProp = serializedObject.FindProperty("debugMode");
                EditorHelper.DrawProperty(debugModeProp.propertyPath, serializedObject);

                if (debugModeProp.boolValue)
                {
                    DrawFields();
                    DrawButtons();
                }
            });
        }

        private void DrawFields()
        {
            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperty("stepDelay", serializedObject);
            });
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
